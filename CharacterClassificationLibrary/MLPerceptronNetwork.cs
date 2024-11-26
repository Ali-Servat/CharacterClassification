using CharacterClassification;
using CharacterClassificationLibrary;
using System.ComponentModel;

public class MLPerceptronNetwork : INotifyPropertyChanged, INeuralNetwork
{
    public int[,] TrainingInputs { get; private set; }
    public int[,] TrainingTargets { get; private set; }
    public int[,] ValidationInputs { get; private set; }
    public int[,] ValidationTargets { get; private set; }
    public double[][,] LinkWeights { get; private set; }
    public Neuron[][] Neurons { get; private set; }
    public double[][,] WeightAdjustmentParameters { get; private set; }
    public double[][] DeltaFactors { get; private set; }
    public double LearningRate { get; private set; }
    public double LastMSE { get; private set; }
    public int MaxEpoch { get; private set; } = 1000;
    public int MaxValidationChecks { get; private set; } = 6;
    private int epoch;
    private int validationChecks;
    public int Epoch
    {
        get { return epoch; }
        set
        {
            epoch = value;
            PropertyChangedEventArgs args = new PropertyChangedEventArgs(nameof(Epoch));
            PropertyChanged?.Invoke(this, args);
        }
    }
    public int ValidationChecks
    {
        get { return validationChecks; }
        set
        {
            validationChecks = value;
            PropertyChangedEventArgs args = new PropertyChangedEventArgs(nameof(ValidationChecks));
            PropertyChanged?.Invoke(this, args);
        }
    }
    public MLPerceptronNetwork(int[,] trainingInputs, int[,] validationInputs, int layerCount, double learningRate, int numOfHiddenNeuronsPerLayer)
    {
        LearningRate = learningRate;
        TrainingInputs = Utils.ExtractInputs(trainingInputs);
        ValidationInputs = Utils.ExtractInputs(validationInputs);
        TrainingTargets = Utils.ExtractTargets(trainingInputs);
        ValidationTargets = Utils.ExtractTargets(validationInputs);
        Epoch = 0;

        LinkWeights = new double[layerCount][,];
        WeightAdjustmentParameters = new double[layerCount][,];
        Neurons = new Neuron[layerCount + 1][];
        DeltaFactors = new double[layerCount + 1][];

        // initialize neurons
        for (int i = 0; i < layerCount + 1; i++)
        {
            if (i == 0)
            {
                Neuron[] inputNeurons = new Neuron[TrainingInputs.GetLength(1)];
                for (int j = 0; j < inputNeurons.Length; j++)
                {
                    Neuron newNeuron = new();
                    inputNeurons[j] = newNeuron;

                }
                inputNeurons[inputNeurons.Length - 1].ActivityLevel = 1;
                Neurons[0] = inputNeurons;
            }
            else if (i == layerCount)
            {
                Neuron[] outputNeurons = new Neuron[TrainingTargets.GetLength(1)];
                for (int j = 0; j < outputNeurons.Length; j++)
                {
                    Neuron newNeuron = new();
                    outputNeurons[j] = newNeuron;

                }
                Neurons[layerCount] = outputNeurons;
            }
            else
            {
                Neuron[] hiddenLayerNeurons = new Neuron[numOfHiddenNeuronsPerLayer];
                for (int j = 0; j < hiddenLayerNeurons.Length; j++)
                {
                    Neuron newNeuron = new();
                    hiddenLayerNeurons[j] = newNeuron;
                }
                hiddenLayerNeurons[hiddenLayerNeurons.Length - 1].ActivityLevel = 1;
                Neurons[i] = hiddenLayerNeurons;
            }
            DeltaFactors[i] = new double[Neurons[i].Length];
        }

        //initialize links
        for (int i = 0; i < layerCount; i++)
        {
            double[,] currentLayerWeights = new double[Neurons[i].Length, Neurons[i + 1].Length];
            WeightAdjustmentParameters[i] = new double[Neurons[i].Length, Neurons[i + 1].Length];

            for (int j = 0; j < Neurons[i].Length; j++)
            {
                for (int k = 0; k < Neurons[i + 1].Length; k++)
                {
                    currentLayerWeights[j, k] = 0;
                }
            }
            LinkWeights[i] = currentLayerWeights;
        }
    }



    public void Train()
    {
        Random rnd = new Random();

        // populate link weights
        for (int i = 0; i < LinkWeights.Length; i++)
        {
            for (int j = 0; j < Neurons[i].Length; j++)
            {
                for (int k = 0; k < Neurons[i + 1].Length; k++)
                {
                    LinkWeights[i][j, k] = rnd.NextDouble() - 0.5;
                }
            }
        }

        bool shouldStop = false;
        while (!shouldStop)
        {
            Epoch++;
            for (int i = 0; i < TrainingInputs.GetLength(0); i++)
            {
                double[] currentRow = new double[TrainingInputs.GetLength(1)];
                for (int j = 0; j < TrainingInputs.GetLength(1); j++)
                {
                    currentRow[j] = TrainingInputs[i, j];
                }

                FeedForward(currentRow);

                int[] targets = new int[TrainingTargets.GetLength(1)];
                for (int j = 0; j < targets.Length; j++)
                {
                    targets[j] = TrainingTargets[i, j];
                }

                BackPropagate(targets);
            }
            shouldStop = CheckStopCondition();
        }
    }
    private bool CheckStopCondition()
    {
        double minError = 1.0e-3;
        double meanSquareError = 0;

        for (int i = 0; i < ValidationInputs.GetLength(0); i++)
        {
            double[] currentRow = new double[ValidationInputs.GetLength(1)];
            for (int j = 0; j < currentRow.Length; j++)
            {
                currentRow[j] = ValidationInputs[i, j];
            }
            FeedForward(currentRow);

            double averageSquaredError = 0;
            for (int k = 0; k < ValidationTargets.GetLength(1); k++)
            {
                double predictedValue = Neurons[Neurons.Length - 1][k].ActivityLevel;
                averageSquaredError += Math.Pow(ValidationTargets[i, k] - predictedValue, 2);
            }
            averageSquaredError /= ValidationTargets.GetLength(1);
            meanSquareError += averageSquaredError;
        }
        meanSquareError /= ValidationTargets.GetLength(0);

        if (Epoch == MaxEpoch || ValidationChecks == MaxValidationChecks || meanSquareError <= minError)
        {
            return true;
        }
        if (meanSquareError > LastMSE)
        {
            ValidationChecks++;
        }
        LastMSE = meanSquareError;
        return false;

    }
    private void FeedForward(double[] currentRow)
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
                    netInput += Neurons[i - 1][k].ActivityLevel * LinkWeights[i - 1][k, j];
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
                netInput += Neurons[Neurons.Length - 2][j].ActivityLevel * LinkWeights[LinkWeights.Length - 1][j, i];
            }
            Neurons[Neurons.Length - 1][i].NetInput = netInput;
            Neurons[Neurons.Length - 1][i].ActivityLevel = TransferFunction(netInput);
        }
    }
    private void BackPropagate(int[] targets)
    {
        int outputLayerIndex = Neurons.Length - 1;
        for (int i = 0; i < Neurons[outputLayerIndex].Length; i++)
        {
            double target = targets[i];
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
                    weightedInputDeltaSum += DeltaFactors[i + 1][k] * LinkWeights[i][j, k];
                }
                DeltaFactors[i][j] = weightedInputDeltaSum * TransferFunctionDerivative(Neurons[i][j].NetInput);

                for (int k = 0; k < Neurons[i - 1].Length; k++)
                {
                    double weightAdjustmentParameter = LearningRate * DeltaFactors[i][j] * Neurons[i - 1][k].ActivityLevel;
                    WeightAdjustmentParameters[i - 1][k, j] = weightAdjustmentParameter;
                }
            }
        }

        for (int i = LinkWeights.Length - 1; i >= 0; i--)
        {
            for (int j = 0; j < Neurons[i].Length; j++)
            {
                for (int k = 0; k < Neurons[i + 1].Length; k++)
                {
                    LinkWeights[i][j, k] += WeightAdjustmentParameters[i][j, k];
                }
            }
        }
    }
    public double Classify(int[] inputs)
    {
        double[] output = new double[2];

        double[] testInputs = new double[inputs.Length];
        for (int i = 0; i < testInputs.Length; i++)
        {
            testInputs[i] = Convert.ToDouble(inputs[i]);
        }
        FeedForward(testInputs);

        for (int i = 0; i < Neurons[Neurons.Length - 1].Length; i++)
        {
            output[i] = (Neurons[Neurons.Length - 1][i].ActivityLevel);
        }

        return output[0] >= output[1] ? 1 : -1;
    }

    private static double TransferFunction(double netInput)
    {
        return (2 / (1 + Math.Pow(Math.E, -netInput))) - 1;
    }
    private static double TransferFunctionDerivative(double netInput)
    {
        return 0.5 * (1 + TransferFunction(netInput) * (1 - TransferFunction(netInput)));
    }
    public event PropertyChangedEventHandler? PropertyChanged;
}