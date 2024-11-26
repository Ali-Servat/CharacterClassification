namespace CharacterClassification
{
    public class SLMOPerceptronNetwork : INeuralNetwork
    {
        public int[,] Dataset { get; set; }
        public Neuron[] InputNeurons { get; set; }
        public Neuron[] OutputNeurons { get; set; }
        public Neuron BiasNeuron { get; set; }
        public Edge[,] Edges { get; set; }
        public double LearningRate { get; set; }
        public double[,] WeightsInLastEpoch { get; set; }

        public SLMOPerceptronNetwork(int[,] dataset, double learningRate)
        {
            Dataset = dataset;
            LearningRate = learningRate;

            int inputDataLength = Dataset.GetLength(1) - 1;
            InputNeurons = new Neuron[inputDataLength];
            OutputNeurons = new Neuron[2];
            BiasNeuron = new Neuron();
            BiasNeuron.ActivityLevel = 1;
            Edges = new Edge[OutputNeurons.Length, InputNeurons.Length + 1];

            // populate input neurons
            for (int i = 0; i < InputNeurons.Length; i++)
            {
                Neuron newNeuron = new Neuron();
                InputNeurons[i] = newNeuron;
            }

            // populate output neurons
            for (int i = 0; i < OutputNeurons.Length; i++)
            {
                OutputNeurons[i] = new Neuron();
            }

            // populate edges
            for (int i = 0; i < OutputNeurons.Length; i++)
            {
                for (int j = 0; j < InputNeurons.Length; j++)
                {
                    Edges[i, j] = new Edge(InputNeurons[j], OutputNeurons[i]);
                }
                Edges[i, inputDataLength] = new Edge(BiasNeuron, OutputNeurons[i]);
            }

            WeightsInLastEpoch = new double[OutputNeurons.Length, inputDataLength + 1];
        }

        public void Train()
        {
            int inputDataLength = Dataset.GetLength(1) - 1;
            for (int i = 0; i < OutputNeurons.Length; i++)
            {
                for (int j = 0; j < inputDataLength + 1; j++)
                {
                    Edges[i, j].Weight = 0;
                }
            }

            bool shouldStop = false;
            while (!shouldStop)
            {
                int datasetCount = Dataset.GetLength(0);

                for (int i = 0; i < datasetCount; i++)
                {
                    for (int j = 0; j < inputDataLength; j++)
                    {
                        InputNeurons[j].ActivityLevel = Dataset[i, j];
                    }

                    // calculate net input and activity level for each output neuron
                    for (int j = 0; j < OutputNeurons.Length; j++)
                    {
                        double netInput = 0;
                        for (int k = 0; k < inputDataLength; k++)
                        {
                            netInput += InputNeurons[k].ActivityLevel * Edges[j, k].Weight;
                        }
                        netInput += BiasNeuron.ActivityLevel * Edges[j, inputDataLength].Weight;
                        OutputNeurons[j].NetInput = netInput;
                        OutputNeurons[j].ActivityLevel = TransferFunction(OutputNeurons[j].NetInput);
                    }

                    int[] targets = new int[OutputNeurons.Length];
                    targets[0] = Dataset[i, inputDataLength];
                    targets[1] = -targets[0];

                    for (int j = 0; j < OutputNeurons.Length; j++)
                    {
                        if (OutputNeurons[j].ActivityLevel != targets[j])
                        {
                            int edgesPerOutputNeuron = Edges.GetLength(1);
                            for (int k = 0; k < edgesPerOutputNeuron - 1; k++)
                            {
                                Edges[j, k].Weight += LearningRate * InputNeurons[k].ActivityLevel * targets[j];
                            }
                            Edges[j, inputDataLength].Weight += LearningRate * BiasNeuron.ActivityLevel * targets[j];
                        }
                    }
                }

                shouldStop = CheckStopCondition();

                for (int i = 0; i < OutputNeurons.Length; i++)
                {
                    for (int j = 0; j < inputDataLength + 1; j++)
                    {
                        WeightsInLastEpoch[i, j] = Edges[i, j].Weight;
                    }
                }
            }
        }
        private bool CheckStopCondition()
        {
            int inputDataLength = Dataset.GetLength(1) - 1;
            double[,] currentEdgeWeights = new double[OutputNeurons.Length, inputDataLength + 1];

            for (int i = 0; i < OutputNeurons.Length; i++)
            {
                for (int j = 0; j < inputDataLength + 1; j++)
                {
                    currentEdgeWeights[i, j] = Edges[i, j].Weight;
                }
            }

            return currentEdgeWeights.Cast<double>().SequenceEqual(WeightsInLastEpoch.Cast<double>());
        }
        public double Classify(int[] input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                InputNeurons[i].ActivityLevel = input[i];
            }

            double netInput = 0;
            for (int i = 0; i < InputNeurons.Length; i++)
            {
                netInput += InputNeurons[i].ActivityLevel * Edges[0, i].Weight;
            }
            netInput += BiasNeuron.ActivityLevel * Edges[0, InputNeurons.Length].Weight;

            return TransferFunction(netInput);
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
