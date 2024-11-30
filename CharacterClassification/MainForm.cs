using CharacterClassification;
using CharacterClassificationLibrary;
using ThyroidClassificationUI;

namespace CharacterDatasetGenerator
{
    public partial class MainForm : Form
    {
        INeuralNetwork? NeuralNetwork;
        private EvaluationTable EvaluationTable;
        public MainForm()
        {
            InitializeComponent();
            NetworkComboBox.DataSource = Enum.GetValues(typeof(NeuralNetworkType));
            SetupConfusionMatrix();
            EvaluationTable = new();
            EvaluationTable.Parent = this;
            EvaluationTable.Location = new Point(480, 320);
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            bool[,] data = new bool[10, 10];

            Grid.Generate(data, this);
        }
        private void TrainButton_Click(object sender, MouseEventArgs e)
        {
            int[,] dataset = ImportData("Dataset.txt");
            ShuffleRows(dataset);

            int totalSamplesCount = dataset.GetLength(0);
            int columnsCount = dataset.GetLength(1);

            int validationDataSize = (int)(0.1 * totalSamplesCount);
            int testDataSize = (int)(0.3 * totalSamplesCount);
            int trainingDataSize = totalSamplesCount - validationDataSize - testDataSize;

            int[,] trainingData = new int[trainingDataSize, columnsCount];
            int[,] validationData = new int[validationDataSize, columnsCount];
            int[,] testData = new int[testDataSize, columnsCount];

            for (int i = 0; i < validationDataSize; i++)
            {
                for (int j = 0; j < columnsCount; j++)
                {
                    validationData[i, j] = dataset[i, j];
                }
            }

            for (int i = 0; i < testDataSize; i++)
            {
                for (int j = 0; j < columnsCount; j++)
                {
                    testData[i, j] = dataset[validationDataSize + i, j];
                }
            }

            for (int i = 0; i < trainingDataSize; i++)
            {
                for (int j = 0; j < columnsCount; j++)
                {
                    trainingData[i, j] = dataset[validationDataSize + testDataSize + i, j];
                }
            }

            NeuralNetworkType selectedNeuralNetwork = (NeuralNetworkType)Enum.Parse(typeof(NeuralNetworkType), NetworkComboBox.Text);

            NeuralNetwork = ChooseNeuralNetwork(selectedNeuralNetwork, trainingData, validationData);
            NeuralNetwork.Train();

            int[] classificationResults = new int[testData.GetLength(0)];
            int testDataRows = testData.GetLength(0);
            int testDataColumns = testData.GetLength(1);

            for (int i = 0; i < testDataRows; i++)
            {
                int[] testCase = new int[testDataColumns - 1];
                for (int j = 0; j < testDataColumns - 1; j++)
                {
                    testCase[j] = testData[i, j];
                }

                double classificationResult = NeuralNetwork.Classify(testCase);
                int isX = selectedNeuralNetwork == NeuralNetworkType.MLPerceptronNetwork ? ConvertNNOutputToInt(classificationResult) : (int)classificationResult;
                classificationResults[i] = isX;
            }

            var testTargets = Utils.ExtractTargets(testData);
            double[][] predictions = new double[classificationResults.Length][];
            for (int i = 0; i < predictions.GetLength(0); i++)
            {
                predictions[i] = new double[2];
                predictions[i][0] = classificationResults[i];
                predictions[i][1] = -predictions[i][0];
            }
            var confusionMatrix = ConstructConfusionMatrix(testTargets, predictions);
            UpdateConfusionMatrix(confusionMatrix);
            var evaluation = Evaluate(confusionMatrix);
            EvaluationTable.UpdateTable(evaluation);
            UpdateMacroScores(evaluation);
        }

        private void UpdateMacroScores(double[,] evaluation)
        {
            double[] macroScores = new double[evaluation.GetLength(1)];
            for (int i = 0; i < evaluation.GetLength(1); i++)
            {
                double average = 0;
                for (int j = 0; j < evaluation.GetLength(0); j++)
                {
                    average += evaluation[j, i];
                }
                average /= 2;
                macroScores[i] = average;
            }

            MacroPrecisionValue.Text = (macroScores[0] * 100).ToString("F2") + " %";
            MacroRecallValue.Text = (macroScores[1] * 100).ToString("F2") + " %";
            MacroAccuracyValue.Text = (macroScores[2] * 100).ToString("F2") + " %";
            MacroF1ScoreValue.Text = (macroScores[3] * 100).ToString("F2") + " %";
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void SaveButton_Click(object sender, EventArgs e)
        {
            bool isX = XRadio.Checked;
            bool shouldSaveToDataset = saveToDatasetCheckbox.Checked;

            Grid.SaveOutputData(isX, shouldSaveToDataset);
            DataSavedLabel.Visible = true;

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 2000;
            timer.Start();
            timer.Tick += new EventHandler(OnTimerTick);
        }
        private void ClearButton_Click(object sender, EventArgs e)
        {
            Grid.ClearGrid();
        }
        private void OnTimerTick(object sender, EventArgs e)
        {
            DataSavedLabel.Visible = false;
        }
        private INeuralNetwork ChooseNeuralNetwork(NeuralNetworkType neuralNetworkType, int[,] dataset, int[,] validationDataset)
        {
            switch (neuralNetworkType)
            {
                case NeuralNetworkType.HebbNetwork:
                    return new HebbNetwork(dataset);
                case NeuralNetworkType.SLSOPerceptronNetwork:
                    return new SLSOPerceptronNetwork(dataset, 0.1);
                case NeuralNetworkType.SLMOPerceptronNetwork:
                    return new SLMOPerceptronNetwork(dataset, 0.1);
                case NeuralNetworkType.AdalineNetwork:
                    return new AdalineNetwork(dataset);
                case NeuralNetworkType.MLPerceptronNetwork:
                    return new MLPerceptronNetwork(dataset, validationDataset, 2, 0.001, 10);
                case NeuralNetworkType.ConvNet:
                    return new ConvNet(dataset);
                default:
                    throw new Exception("Unknown type of neural network.");
            }
        }
        private int ConvertNNOutputToInt(double output)
        {
            if (output >= 0)
            {
                return 1;
            }
            return -1;
        }
        private int[,] ImportData(string path)
        {
            var dataPath = "Data/";
            var fullPath = dataPath + path;
            int[,] output;
            using (StreamReader sr = new StreamReader(fullPath))
            {
                int lineCount = GetLineCount(fullPath);
                int dataLength = GetDataLength(fullPath);
                output = new int[lineCount, dataLength];

                string? currentLine = sr.ReadLine();
                int rowCount = 0;

                while (currentLine != null)
                {
                    string[] data = currentLine.Split(",");
                    int[] castedData = new int[data.Length];

                    for (int i = 0; i < data.Length; i++)
                    {
                        castedData[i] = Convert.ToInt32(data[i]);
                    }

                    for (int i = 0; i < data.Length; i++)
                    {
                        output[rowCount, i] = castedData[i];
                    }

                    currentLine = sr.ReadLine();
                    rowCount++;
                }
            }
            return output;
        }
        private int GetLineCount(string path)
        {
            int lineCount = 0;

            using (StreamReader sr = new StreamReader(path))
            {
                string? line = sr.ReadLine();

                while (line != null)
                {
                    lineCount++;
                    line = sr.ReadLine();
                }
            }
            return lineCount;
        }
        private int GetDataLength(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                string? line = sr.ReadLine();
                string[] data = line.Split(",");
                return data.Length;
            }
        }
        private void ClassifyButton_Click(object sender, EventArgs e)
        {
            if (NeuralNetwork == null)
            {
                MessageBox.Show("You have to Train the model first!");
                return;
            }

            bool[] flattenedMap = Grid.GridMap.Cast<bool>().ToArray();
            int[] testInput = new int[flattenedMap.Length];
            for (int i = 0; i < flattenedMap.Length; i++)
            {
                testInput[i] = flattenedMap[i] ? 1 : -1;
            }

            double classificationResult = NeuralNetwork.Classify(testInput);
            char output = classificationResult > 0 ? 'X' : 'O';
            MessageBox.Show($"Classification result: {output}");
        }
        private void SetupConfusionMatrix()
        {
            ConfusionMatrix.DefaultCellStyle = new DataGridViewCellStyle() { SelectionBackColor = Color.White, SelectionForeColor = Color.Black, Font = new Font("Segoe UI", 8) };

            ConfusionMatrix.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            ConfusionMatrix.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;

            ConfusionMatrix.TopLeftHeaderCell.Value = "Actual/Predicted";

            ConfusionMatrix.RowCount = 2;
            ConfusionMatrix.Rows[0].HeaderCell.Value = "X";
            ConfusionMatrix.Rows[1].HeaderCell.Value = "O";
        }
        private void UpdateConfusionMatrix(int[,] confusionMatrix)
        {
            for (int i = 0; i < confusionMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < confusionMatrix.GetLength(1); j++)
                {
                    ConfusionMatrix.Rows[i].Cells[j].Value = confusionMatrix[i, j];
                }
            }
        }
        private static int[,] ConstructConfusionMatrix(int[,] testTargets, double[][] classificationResults)
        {
            int[,] confusionMatrix = new int[testTargets.GetLength(1), testTargets.GetLength(1)];
            confusionMatrix[0, 0] = CountPatternMatches([1, -1], [1, -1], testTargets, classificationResults);
            confusionMatrix[0, 1] = CountPatternMatches([1, -1], [-1, 1], testTargets, classificationResults);
            confusionMatrix[1, 0] = CountPatternMatches([-1, 1], [1, -1], testTargets, classificationResults);
            confusionMatrix[1, 1] = CountPatternMatches([-1, 1], [-1, 1], testTargets, classificationResults);
            return confusionMatrix;
        }
        private static int CountPatternMatches(int[] targetPattern, int[] predictionPattern, int[,] targets, double[][] predictions)
        {
            int counter = 0;
            for (int i = 0; i < targets.GetLength(0); i++)
            {
                int[] currentTargetsRow = new int[targets.GetLength(1)];
                for (int j = 0; j < currentTargetsRow.Length; j++)
                {
                    currentTargetsRow[j] = targets[i, j];
                }
                int[] currentPredictionRow = new int[targets.GetLength(1)];
                for (int j = 0; j < currentPredictionRow.Length; j++)
                {
                    currentPredictionRow[j] = Convert.ToInt32(predictions[i][j]);
                }

                if (Enumerable.SequenceEqual(currentTargetsRow, targetPattern) && Enumerable.SequenceEqual(currentPredictionRow, predictionPattern))
                {
                    counter++;
                }
            }
            return counter;
        }
        private double[,] Evaluate(int[,] confusionMatrix)
        {
            double[,] output = new double[EvaluationTable.RowCount, EvaluationTable.ColumnCount];

            int totalCount = 0;
            for (int i = 0; i < confusionMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < confusionMatrix.GetLength(1); j++)
                {
                    totalCount += confusionMatrix[i, j];
                }
            }

            for (int i = 0; i < confusionMatrix.GetLength(0); i++)
            {
                double tpCount = 0;
                double tnCount = 0;
                double fpCount = 0;
                double fnCount = 0;

                int tpIndex = i;
                tpCount = confusionMatrix[i, tpIndex];

                for (int j = 0; j < confusionMatrix.GetLength(1); j++)
                {
                    fpCount = j == i ? fpCount : fpCount + confusionMatrix[i, j];
                }

                for (int j = 0; j < confusionMatrix.GetLength(0); j++)
                {
                    fnCount = j == i ? fnCount : fnCount + confusionMatrix[j, i];
                }

                tnCount = totalCount - tpCount - fnCount - fpCount;
                output[i, 0] = tpCount / (tpCount + fpCount);
                output[i, 1] = (tpCount + fnCount == 0) ? 0 : tpCount / (tpCount + fnCount);
                output[i, 2] = (tpCount + tnCount) / totalCount;
                output[i, 3] = (output[i, 0] + output[i, 1] == 0) ? 0 : (2 * output[i, 0] * output[i, 1]) / (output[i, 0] + output[i, 1]);
            }
            return output;
        }
        static void ShuffleRows<T>(T[,] array)
        {
            Random rng = new Random();
            int rowCount = array.GetLength(0);
            int colCount = array.GetLength(1);

            for (int i = rowCount - 1; i > 0; i--)
            {
                int j = rng.Next(0, i + 1);

                for (int k = 0; k < colCount; k++)
                {
                    T temp = array[i, k];
                    array[i, k] = array[j, k];
                    array[j, k] = temp;
                }
            }
        }
    }
}
