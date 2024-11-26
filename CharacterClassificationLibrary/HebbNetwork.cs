namespace CharacterClassification
{
    public class HebbNetwork : INeuralNetwork
    {
        public int[,] Dataset { get; set; }
        public Neuron[] InputNeurons { get; set; }
        public Neuron BiasNeuron { get; set; }
        public Neuron OutputNeuron { get; set; }
        public Edge[] Edges { get; set; }

        public HebbNetwork(int[,] dataset)
        {
            Dataset = dataset;
            OutputNeuron = new Neuron();
            BiasNeuron = new Neuron();
            BiasNeuron.ActivityLevel = 1;

            int inputDataLength = Dataset.GetLength(1) - 1;
            InputNeurons = new Neuron[inputDataLength];
            Edges = new Edge[inputDataLength + 1];

            for (int i = 0; i < inputDataLength; i++)
            {
                Neuron newNeuron = new Neuron();
                InputNeurons[i] = newNeuron;
                Edges[i] = new Edge(newNeuron, OutputNeuron);
            }
            Edges[inputDataLength] = new Edge(BiasNeuron, OutputNeuron);
        }

        public void Train()
        {
            int inputDataLength = Dataset.GetLength(1) - 1;
            int datasetCount = Dataset.GetLength(0);

            foreach (Edge edge in Edges)
            {
                edge.Weight = 0;
            }

            for (int i = 0; i < datasetCount; i++)
            {
                // populate input neurons
                for (int j = 0; j < inputDataLength; j++)
                {
                    InputNeurons[j].ActivityLevel = Dataset[i, j];
                }

                int target = Dataset[i, inputDataLength];
                OutputNeuron.ActivityLevel = target;

                // update edge weights
                for (int j = 0; j < Edges.Length - 1; j++)
                {
                    Edges[j].Weight += (InputNeurons[j].ActivityLevel * OutputNeuron.ActivityLevel);
                }
                Edges[inputDataLength].Weight += BiasNeuron.ActivityLevel * OutputNeuron.ActivityLevel;
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
            for (int i = 0; i < InputNeurons.Length; i++)
            {
                netInput += InputNeurons[i].ActivityLevel * Edges[i].Weight;
            }
            netInput += BiasNeuron.ActivityLevel * Edges[inputDataLength].Weight;

            return TransferFunction(netInput);
        }

        private int TransferFunction(double netInput)
        {
            if (netInput >= 0)
            {
                return 1;
            }
            return -1;
        }
    }
}

