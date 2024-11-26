using CharacterClassification;

namespace CharacterClassificationLibrary
{
    public class AdalineNetwork : INeuralNetwork
    {
        public int[,] Dataset { get; private set; }
        public double[] Weights { get; private set; }
        public double Bias { get; private set; }
        public double LearningRate { get; private set; }
        public int MaxEpochs { get; private set; }
        public double Threshold { get; private set; }

        public AdalineNetwork(int[,] dataset, double learningRate = 0.01, int maxEpochs = 1000, double threshold = 0.001)
        {
            Dataset = dataset;
            Weights = new double[dataset.GetLength(1)];
            Bias = 0.0;
            LearningRate = learningRate;
            MaxEpochs = maxEpochs;
            Threshold = threshold;

            Random rnd = new Random();
            for (int i = 0; i < Weights.Length; i++)
            {
                Weights[i] = rnd.NextDouble() * 0.1 - 0.05;
            }
            Bias = rnd.NextDouble() * 0.1 - 0.05;
        }

        public void Train()
        {
            int numSamples = Dataset.GetLength(0);
            var inputs = new double[numSamples][];
            var targets = new double[numSamples];

            for (int i = 0; i < numSamples; i++)
            {
                inputs[i] = new double[Dataset.GetLength(1) - 1];
                for (int j = 0; j < Dataset.GetLength(1) - 1; j++)
                {
                    inputs[i][j] = Dataset[i, j];
                }
            }

            for (int i = 0; i < numSamples; i++)
            {
                targets[i] = Dataset[i, Dataset.GetLength(1) - 1];
            }

            for (int epoch = 0; epoch < MaxEpochs; epoch++)
            {
                double largestWeightChange = 0.0;

                for (int sampleIndex = 0; sampleIndex < numSamples; sampleIndex++)
                {
                    double[] input = inputs[sampleIndex];
                    double target = targets[sampleIndex];

                    // Calculate net input
                    double netInput = Bias;
                    for (int i = 0; i < input.Length; i++)
                    {
                        netInput += Weights[i] * input[i];
                    }

                    double error = target - netInput;

                    // Update weights and bias
                    for (int i = 0; i < input.Length; i++)
                    {
                        double weightChange = LearningRate * error * input[i];
                        Weights[i] += weightChange;

                        if (Math.Abs(weightChange) > largestWeightChange)
                        {
                            largestWeightChange = Math.Abs(weightChange);
                        }
                    }
                    Bias += LearningRate * error;
                }

                if (largestWeightChange < Threshold)
                {
                    break;
                }
            }
        }

        public double Classify(int[] input)
        {
            double netInput = Bias;
            for (int i = 0; i < input.Length; i++)
            {
                netInput += Weights[i] * input[i];
            }
            return netInput >= 0 ? 1 : -1;
        }
    }
}
