namespace CharacterClassification
{
    public class Edge
    {
        public Neuron Source { get; set; }
        public Neuron Destination { get; set; }
        public double Weight { get; set; }

        public Edge(Neuron source, Neuron destination)
        {
            Source = source;
            Destination = destination;
        }
    }
}
