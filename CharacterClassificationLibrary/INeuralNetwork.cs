namespace CharacterClassification
{
    public interface INeuralNetwork
    {
        void Train();
        double Classify(int[] input);
    }
}
