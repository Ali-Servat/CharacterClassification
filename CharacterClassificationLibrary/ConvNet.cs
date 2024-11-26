using CharacterClassification;
using TorchSharp.Modules;
using static TorchSharp.torch;

namespace CharacterClassificationLibrary
{
    public class ConvNet : INeuralNetwork
    {
        private Sequential Model;
        private Tensor Features;
        private Tensor Targets;

        public ConvNet(int[,] dataset)
        {
            int[,] inputs = Utils.ExtractInputs(dataset);
            int[] targets = new int[dataset.GetLength(0)];
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i] = dataset[i, 100] == -1 ? 0 : 1;
            }

            Features = tensor(inputs).reshape(inputs.GetLength(0), 1, 10, 10).to(float32);
            Targets = tensor(targets).to(int64);

            Model = nn.Sequential(
                                ("conv1", nn.Conv2d(1, 32, 3)),
                                ("relu1", nn.ReLU()),
                                ("maxpool1", nn.MaxPool2d(2, 2)),
                                ("conv2", nn.Conv2d(32, 64, 3)),
                                ("relu2", nn.ReLU()),
                                ("flatten", nn.Flatten()),
                                ("linear1", nn.Linear(64 * 2 * 2, 64)),
                                ("relu3", nn.ReLU()),
                                ("linear2", nn.Linear(64, 2))
                                );
        }

        public void Train()
        {
            var optimizer = optim.Adam(Model.parameters());
            var lossFn = nn.CrossEntropyLoss();

            int epochs = 100;
            for (int epoch = 1; epoch <= epochs; epoch++)
            {
                optimizer.zero_grad();
                var output = Model.forward(Features);
                var loss = lossFn.forward(output, Targets);
                loss.backward();
                optimizer.step();
            }
        }

        public double Classify(int[] input)
        {
            var inputTensor = tensor(input).reshape(1, 1, 10, 10).to(float32);
            var output = Model.forward(inputTensor);
            var prediction = output.argmax(1).item<long>();
            return prediction == 0 ? -1 : 1;
        }
    }
}
