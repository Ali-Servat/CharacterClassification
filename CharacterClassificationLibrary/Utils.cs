namespace CharacterClassificationLibrary
{
    public class Utils
    {
        public static int[,] ExtractInputs(int[,] dataset)
        {
            int[,] inputs = new int[dataset.GetLength(0), dataset.GetLength(1) - 1];
            for (int i = 0; i < inputs.GetLength(0); i++)
            {
                for (int j = 0; j < inputs.GetLength(1); j++)
                {
                    inputs[i, j] = dataset[i, j];
                }
            }
            return inputs;
        }

        public static int[,] ExtractTargets(int[,] dataset)
        {
            int[,] targets = new int[dataset.GetLength(0), 2];
            for (int i = 0; i < targets.GetLength(0); i++)
            {
                targets[i, 0] = (int)dataset[i, dataset.GetLength(1) - 1];
                targets[i, 1] = -targets[i, 0];
            }
            return targets;
        }
    }
}
