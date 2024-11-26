namespace CharacterClassification
{
    public class SLSOPerceptronNetwork : INeuralNetwork
    {
        public int[,] Dataset { get; set; }
        public Neuron[] InputNeurons { get; set; }
        public Neuron BiasNeuron { get; set; }
        public Neuron OutputNeuron { get; set; }
        public Edge[] Edges { get; set; }
        public double LearningRate { get; set; }
        public double[] WeightsInLastEpoch { get; set; }

        public SLSOPerceptronNetwork(int[,] dataset, double learningRate)
        {
            Dataset = dataset;
            LearningRate = learningRate;

            int inputDataLength = Dataset.GetLength(1) - 1;
            OutputNeuron = new Neuron();
            BiasNeuron = new Neuron();
            BiasNeuron.ActivityLevel = 1;
            InputNeurons = new Neuron[inputDataLength];
            Edges = new Edge[inputDataLength + 1];

            for (int i = 0; i < inputDataLength; i++)
            {
                Neuron newNeuron = new Neuron();
                InputNeurons[i] = newNeuron;
                Edges[i] = new Edge(newNeuron, OutputNeuron);
            }
            Edges[inputDataLength] = new Edge(BiasNeuron, OutputNeuron);

            WeightsInLastEpoch = new double[Edges.Length];
        }

        public void Train()
        {
            foreach (Edge edge in Edges)
            {
                edge.Weight = 0;
            }
            int datasetCount = Dataset.GetLength(0);
            int inputDatasetLength = Dataset.GetLength(1) - 1;

            bool shouldStop = false;
            while (!shouldStop)
            {
                for (int i = 0; i < datasetCount; i++)
                {
                    // populate input neurons
                    for (int j = 0; j < inputDatasetLength; j++)
                    {
                        InputNeurons[j].ActivityLevel = Dataset[i, j];
                    }

                    // calculate net input
                    double netInput = 0;
                    for (int j = 0; j < InputNeurons.Length; j++)
                    {
                        netInput += InputNeurons[j].ActivityLevel * Edges[j].Weight;
                    }
                    netInput += BiasNeuron.ActivityLevel * Edges[inputDatasetLength].Weight;

                    OutputNeuron.ActivityLevel = TransferFunction(netInput);

                    int target = Dataset[i, inputDatasetLength];
                    if (OutputNeuron.ActivityLevel != target)
                    {
                        // update edge weights
                        for (int j = 0; j < Edges.Length - 1; j++)
                        {
                            Edges[j].Weight += LearningRate * InputNeurons[j].ActivityLevel * target;
                        }
                        Edges[inputDatasetLength].Weight += LearningRate * BiasNeuron.ActivityLevel * target;
                    }
                }

                shouldStop = CheckStopCondition();

                for (int i = 0; i < Edges.Length; i++)
                {
                    WeightsInLastEpoch[i] = Edges[i].Weight;
                }
            }
        }

        public double Classify(int[] input)
        {
            int inputDataLength = Dataset.GetLength(1) - 1;
            for (int i = 0; i < input.Length; i++)
            {
                InputNeurons[i].ActivityLevel = input[i];
            }

            double netInput = 0;
            for (int i = 0; i < inputDataLength; i++)
            {
                netInput += InputNeurons[i].ActivityLevel * Edges[i].Weight;
            }
            netInput += BiasNeuron.ActivityLevel * Edges[inputDataLength].Weight;

            return TransferFunction(netInput);
        }

        private bool CheckStopCondition()
        {
            double[] currentEdgeWeights = new double[Edges.Length];
            for (int i = 0; i < currentEdgeWeights.Length; i++)
            {
                currentEdgeWeights[i] = Edges[i].Weight;
            }

            return Enumerable.SequenceEqual(currentEdgeWeights, WeightsInLastEpoch);
        }

        private int TransferFunction(double netInput)
        {
            double threshold = 0.2;
            if (netInput > threshold)
            {
                return 1;
            }
            else if (netInput <= threshold && netInput >= -threshold)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }
    }
}
