namespace CharacterDatasetGenerator
{
    public class Grid
    {
        public static bool[,] GridMap { get; set; } = new bool[10, 10];
        private static Button[,] Buttons { get; set; } = new Button[10, 10];
        private static string LastSavedPath { get; set; } = "LastSavedData.txt";
        private static string DatasetPath { get; set; } = "Dataset.txt";
        private static bool isPainting;
        public static void Generate(bool[,] gridMap, Form form)
        {
            GridMap = gridMap;
            Panel panel = new Panel();
            panel.Size = new Size(400, 400);
            panel.Location = new Point(20, 60);
            form.Controls.Add(panel);

            for (int i = 0; i < GridMap.GetLength(0); i++)
            {
                for (int j = 0; j < GridMap.GetLength(1); j++)
                {
                    Button button = new Button();
                    button.Cursor = Cursors.Hand;
                    button.Location = CalculateButtonLocation(i, j);
                    button.Size = new Size(40, 40);
                    button.Tag = new Tuple<int, int>(i, j);
                    panel.Controls.Add(button);
                    Buttons[i, j] = button;
                    button.MouseDown += Button_MouseDown;
                    //button.MouseEnter += Button_MouseEnter;
                }
            }
        }
        public static void SaveOutputData(bool isX, bool shouldSaveToDataset)
        {
            string data = CreateOutputData(isX, shouldSaveToDataset);

            using (StreamWriter sw = new StreamWriter(LastSavedPath, false))
            {
                sw.WriteLine(data);
            }

            if (shouldSaveToDataset)
            {
                using (StreamWriter sw = new StreamWriter(DatasetPath, true))
                {
                    sw.WriteLine(data);
                }
            }

            ClearGrid();
        }
        public static void ClearGrid()
        {
            for (int i = 0; i < GridMap.GetLength(0); i++)
            {
                for (int j = 0; j < GridMap.GetLength(1); j++)
                {
                    GridMap[i, j] = false;
                    Buttons[i, j].Tag = new Tuple<int, int>(i, j);
                    Buttons[i, j].BackColor = Color.White;
                }
            }
        }
        public static bool ImportGrid()
        {
            using (StreamReader sr = new StreamReader(LastSavedPath))
            {
                string data = sr.ReadToEnd();
                string[] dataArray = data.Split(",");
                int[,] dataMap = new int[10, 10];
                bool isX = dataArray[100] == "1";

                for (int i = 0; i < dataArray.Length - 1; i++)
                {
                    int row = i / 10;
                    int col = i % 10;

                    GridMap[row, col] = dataArray[i] == "1";
                    Buttons[row, col].BackColor = dataArray[i] == "1" ? Color.Black : Color.White;
                    Buttons[row, col].Tag = new Tuple<int, int>(row, col);
                }

                return isX;
            }
        }

        private static Point CalculateButtonLocation(int i, int j)
        {
            int x;
            int y;

            int modX = i % 10;
            int modY = j % 10;

            x = (modX) * 40;
            y = (modY) * 40;

            return new Point(x, y);
        }

        private static void Button_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isPainting = !isPainting;
                ToggleButton(sender); // Set color when initially clicked
            }
        }

        private static void Button_MouseEnter(object sender, EventArgs e)
        {
            if (isPainting)
            {
                ToggleButton(sender);  // Set color when mouse enters
            }
        }

        private static void ToggleButton(object sender)
        {
            Button button = sender as Button;
            Tuple<int, int> vals = (Tuple<int, int>)button.Tag;
            int i = vals.Item1;
            int j = vals.Item2;

            bool isClicked = GridMap[i, j];

            if (isClicked)
            {
                GridMap[i, j] = false;
                button.BackColor = Color.White;
            }
            else
            {
                GridMap[i, j] = true;
                button.BackColor = Color.Black;
            }
        }
        private static string CreateOutputData(bool isX, bool shouldSaveToDataSet)
        {
            int[] outputArray = new int[101];

            int indexCounter = 0;
            for (int i = 0; i < GridMap.GetLength(0); i++)
            {
                for (int j = 0; j < GridMap.GetLength(1); j++)
                {
                    int indexValue = GridMap[i, j] == false ? -1 : 1;
                    outputArray[indexCounter] = indexValue;
                    indexCounter++;
                }
            }

            if (shouldSaveToDataSet)
            {
                outputArray[100] = isX ? 1 : -1;
            }

            string data = string.Join(",", outputArray);
            return data;
        }
    }
}
