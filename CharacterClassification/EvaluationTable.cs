namespace ThyroidClassificationUI
{
    public class EvaluationTable : DataGridView
    {
        public EvaluationTable()
        {
            RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle { Font = new Font("Segoe UI", 12) };
            RowHeadersDefaultCellStyle = new DataGridViewCellStyle { Font = new Font("Segoe UI", 12) };
            DefaultCellStyle = new DataGridViewCellStyle { Font = new Font("Segoe UI", 12) };
            AllowUserToResizeColumns = false;

            ColumnCount = 4;
            Columns[0].HeaderText = "Precision";
            Columns[1].HeaderText = "Recall";
            Columns[2].HeaderText = "Accuracy";
            Columns[3].HeaderText = "F1 Score";

            RowCount = 2;
            Rows[0].HeaderCell.Value = "X";
            Rows[1].HeaderCell.Value = "O";


            int totalWidth = 0;
            foreach (DataGridViewColumn column in Columns)
            {
                totalWidth += column.Width;
            }
            totalWidth += RowHeadersWidth + 34;
            Width = totalWidth;
            Height = 81;
        }

        public void UpdateTable(double[,] evaluations)
        {
            for (int i = 0; i < evaluations.GetLength(0); i++)
            {
                for (int j = 0; j < evaluations.GetLength(1); j++)
                {
                    Rows[i].Cells[j].Value = evaluations[i, j].ToString("F2");
                }
            }
        }
    }
}
