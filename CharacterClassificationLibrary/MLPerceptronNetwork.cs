using CharacterClassification;
using Microsoft.Extensions.Logging;

namespace CharacterClassificationLibrary
{
    public class MLPerceptronNetwork : INeuralNetwork
    {
        public int[,] Dataset { get; set; }
        public int[,] ValidationData { get; set; }
        public Neuron[][] Neurons { get; set; }
        public Edge[][,] Edges { get; set; }
        public double[][,] WeightAdjustmentParameters { get; set; }
        public double[][] DeltaFactors { get; set; }
        public double LearningRate { get; set; }
        public int Epoch { get; set; }
        public double LastValidationResult { get; set; } = double.PositiveInfinity;

        public MLPerceptronNetwork(int[,] dataset, double learningRate, int layerCount, int[,] validationData)
        {
            Dataset = dataset;
            LearningRate = learningRate;
            ValidationData = validationData;
            Epoch = 1;

            int dataLength = dataset.GetLength(1);
            Neurons = new Neuron[layerCount + 1][];
            Edges = new Edge[layerCount][,];
            WeightAdjustmentParameters = new double[layerCount][,];
            DeltaFactors = new double[layerCount + 1][];

            // initialize neurons
            for (int i = 0; i < layerCount + 1; i++)
            {
                if (i == 0)
                {
                    Neuron[] inputNeurons = new Neuron[dataLength];
                    for (int j = 0; j < inputNeurons.Length; j++)
                    {
                        Neuron newNeuron = new Neuron();
                        inputNeurons[j] = newNeuron;

                    }
                    inputNeurons[inputNeurons.Length - 1].ActivityLevel = 1;
                    Neurons[i] = inputNeurons;
                }
                else if (i == layerCount)
                {
                    Neuron[] outputNeurons = new Neuron[2];
                    for (int j = 0; j < outputNeurons.Length; j++)
                    {
                        Neuron newNeuron = new Neuron();
                        outputNeurons[j] = newNeuron;

                    }
                    Neurons[i] = outputNeurons;
                }
                else
                {
                    Neuron[] hiddenLayerNeurons = new Neuron[(int)(2.0 / 3 * Neurons[0].Length) + 2]; // + 2 is the number of outputs
                    for (int j = 0; j < hiddenLayerNeurons.Length; j++)
                    {
                        Neuron newNeuron = new Neuron();
                        hiddenLayerNeurons[j] = newNeuron;
                    }
                    hiddenLayerNeurons[hiddenLayerNeurons.Length - 1].ActivityLevel = 1;
                    Neurons[i] = hiddenLayerNeurons;
                }
                DeltaFactors[i] = new double[Neurons[i].Length];
            }

            //initialize edges
            for (int i = 0; i < layerCount; i++)
            {
                Edge[,] currentLayerEdges = new Edge[Neurons[i].Length, Neurons[i + 1].Length];
                WeightAdjustmentParameters[i] = new double[Neurons[i].Length, Neurons[i + 1].Length];

                for (int j = 0; j < Neurons[i].Length; j++)
                {
                    for (int k = 0; k < Neurons[i + 1].Length; k++)
                    {
                        currentLayerEdges[j, k] = new Edge(Neurons[i][j], Neurons[i][k]);
                    }
                }
                Edges[i] = currentLayerEdges;
            }
        }

        public void Train()
        {
            Random rnd = new Random();

            // populate edges
            for (int i = 0; i < Edges.Length; i++)
            {
                for (int j = 0; j < Neurons[i].Length; j++)
                {
                    for (int k = 0; k < Neurons[i + 1].Length; k++)
                    {
                        Edges[i][j, k].Weight = rnd.NextDouble() - 0.5;
                    }
                }
            }

            bool shouldStop = false;
            while (!shouldStop)
            {
                for (int i = 0; i < Dataset.GetLength(0); i++)
                {
                    int[] currentRow = new int[Dataset.GetLength(1)];
                    for (int j = 0; j < Dataset.GetLength(1); j++)
                    {
                        currentRow[j] = Dataset[i, j];
                    }

                    FeedForward(currentRow);

                    int[] targets = new int[Neurons[Neurons.Length - 1].Length];
                    targets[0] = Dataset[i, Dataset.GetLength(1) - 1];
                    targets[1] = -targets[0];

                    BackPropagate(targets);
                }
                shouldStop = CheckStopCondition();
                Epoch++;
            }
        }

        private bool CheckStopCondition()
        {
            int maxEpochs = 100;
            int validationInterval = maxEpochs / ValidationData.GetLength(0);
            bool shouldValidate = Epoch % validationInterval == 0;
            int validationIndex = Epoch / validationInterval - 1;

            if (Epoch > maxEpochs)
            {
                return true;
            }

            if (shouldValidate)
            {
                int[] currentRow = new int[ValidationData.GetLength(1)];
                for (int i = 0; i < ValidationData.GetLength(1); i++)
                {
                    currentRow[i] = ValidationData[validationIndex, i];
                }
                double classificationResult = Classify(currentRow);
                double target = currentRow[currentRow.Length - 1];

                double errorRate = Math.Abs(target - classificationResult);

                if (LastValidationResult < errorRate)
                {
                    return true;
                }
                LastValidationResult = errorRate;
            }
            return false;
        }
        private void BackPropagate(int[] targets)
        {
            int outputLayerIndex = Neurons.Length - 1;
            for (int i = 0; i < Neurons[outputLayerIndex].Length; i++)
            {
                double target = targets[i];
                if (i == 1) target = -target;
                Neuron currentNeuron = Neurons[outputLayerIndex][i];
                double errorRate = target - currentNeuron.ActivityLevel;

                double deltaFactor = errorRate * TransferFunctionDerivative(currentNeuron.NetInput);
                DeltaFactors[outputLayerIndex][i] = deltaFactor;

                for (int j = 0; j < Neurons[outputLayerIndex - 1].Length; j++)
                {
                    double weightAdjustmentParameter = LearningRate * DeltaFactors[outputLayerIndex][i] * Neurons[outputLayerIndex - 1][j].ActivityLevel;

                    WeightAdjustmentParameters[outputLayerIndex - 1][j, i] = weightAdjustmentParameter;
                }
            }

            for (int i = outputLayerIndex - 1; i > 0; i--)
            {
                for (int j = 0; j < Neurons[i].Length; j++)
                {
                    double weightedInputDeltaSum = 0;
                    for (int k = 0; k < Neurons[i + 1].Length; k++)
                    {
                        weightedInputDeltaSum += DeltaFactors[i + 1][k] * Edges[i][j, k].Weight;
                    }
                    DeltaFactors[i][j] = weightedInputDeltaSum * TransferFunctionDerivative(Neurons[i][j].NetInput);

                    for (int k = 0; k < Neurons[i - 1].Length; k++)
                    {
                        double weightAdjustmentParameter = LearningRate * DeltaFactors[i][j] * Neurons[i - 1][k].ActivityLevel;
                        WeightAdjustmentParameters[i - 1][k, j] = weightAdjustmentParameter;
                    }
                }
            }

            for (int i = outputLayerIndex - 1; i > 0; i--)
            {
                for (int j = 0; j < Neurons[i].Length; j++)
                {
                    for (int k = 0; k < Neurons[i + 1].Length; k++)
                    {
                        Edges[i][j, k].Weight += WeightAdjustmentParameters[i][j, k];
                    }
                }
            }
        }
        private void FeedForward(int[] currentRow)
        {
            // populate input neurons
            for (int i = 0; i < Neurons[0].Length - 1; i++)
            {
                Neurons[0][i].ActivityLevel = currentRow[i];
            }

            // calculate net input and activity level for each hidden layer neuron
            for (int i = 1; i < Neurons.Length - 1; i++)
            {
                for (int j = 0; j < Neurons[i].Length; j++)
                {
                    double netInput = 0;
                    for (int k = 0; k < Neurons[i - 1].Length; k++)
                    {
                        netInput += Neurons[i - 1][k].ActivityLevel * Edges[i - 1][k, j].Weight;
                    }
                    Neurons[i][j].NetInput = netInput;
                    if (j != Neurons[i].Length - 1)
                    {
                        Neurons[i][j].ActivityLevel = TransferFunction(netInput);
                    }
                }
            }

            // calculate net input and activity level for output neurons
            for (int i = 0; i < Neurons[Neurons.Length - 1].Length; i++)
            {
                double netInput = 0;
                for (int j = 0; j < Neurons[Neurons.Length - 2].Length; j++)
                {
                    netInput += Neurons[Neurons.Length - 2][j].ActivityLevel * Edges[Edges.Length - 1][j, i].Weight;
                }
                Neurons[Neurons.Length - 1][i].NetInput = netInput;
                Neurons[Neurons.Length - 1][i].ActivityLevel = TransferFunction(netInput);
            }
        }
        private double TransferFunction(double netInput)
        {
            return (2 / (1 + Math.Pow(Math.E, -netInput))) - 1;
        }
        private double TransferFunctionDerivative(double netInput)
        {
            return 1.0 / 2 * (1 + TransferFunction(netInput) * (1 - TransferFunction(netInput)));
        }
        public double Classify(int[] input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                Neurons[0][i].ActivityLevel = input[i];
            }

            for (int i = 1; i < Neurons.Length - 1; i++)
            {
                for (int j = 0; j < Neurons[i].Length - 1; j++)
                {
                    double netInput = 0;
                    for (int k = 0; k < Neurons[i - 1].Length; k++)
                    {
                        netInput += Neurons[i - 1][k].ActivityLevel * Edges[i - 1][k, j].Weight;
                    }
                    Neurons[i][j].NetInput = netInput;
                    Neurons[i][j].ActivityLevel = TransferFunction(netInput);
                }
            }

            for (int i = 0; i < Neurons[Neurons.Length - 1].Length; i++)
            {
                double netInput = 0;
                for (int j = 0; j < Neurons[Neurons.Length - 2].Length; j++)
                {
                    netInput += Neurons[Neurons.Length - 2][j].ActivityLevel * Edges[Edges.Length - 1][j, i].Weight;
                }
                Neurons[Neurons.Length - 1][i].NetInput = netInput;
                Neurons[Neurons.Length - 1][i].ActivityLevel = TransferFunction(netInput);
            }

            return Neurons[Neurons.Length - 1][0].ActivityLevel;
        }
    }
}

