﻿namespace CharacterDatasetGenerator
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            appLabel = new Label();
            SaveButton = new Button();
            XRadio = new RadioButton();
            ORadio = new RadioButton();
            saveToDatasetCheckbox = new CheckBox();
            ClearButton = new Button();
            DataSavedLabel = new Label();
            NetworkComboBox = new ComboBox();
            SelectedNetworkLabel = new Label();
            TrainButton = new Button();
            ClassifyButton = new Button();
            ConfusionMatrix = new DataGridView();
            X = new DataGridViewTextBoxColumn();
            O = new DataGridViewTextBoxColumn();
            label1 = new Label();
            label2 = new Label();
            MacroPrecisionValue = new Label();
            MacroRecallValue = new Label();
            label5 = new Label();
            MacroAccuracyValue = new Label();
            label7 = new Label();
            MacroF1ScoreValue = new Label();
            label9 = new Label();
            ((System.ComponentModel.ISupportInitialize)ConfusionMatrix).BeginInit();
            SuspendLayout();
            // 
            // appLabel
            // 
            appLabel.AutoSize = true;
            appLabel.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            appLabel.Location = new Point(20, 20);
            appLabel.Margin = new Padding(5, 0, 5, 0);
            appLabel.Name = "appLabel";
            appLabel.Size = new Size(241, 30);
            appLabel.TabIndex = 0;
            appLabel.Text = "Character Classification";
            // 
            // SaveButton
            // 
            SaveButton.BackColor = SystemColors.Window;
            SaveButton.Cursor = Cursors.Hand;
            SaveButton.Font = new Font("Segoe UI", 14F);
            SaveButton.Location = new Point(27, 570);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new Size(73, 34);
            SaveButton.TabIndex = 100;
            SaveButton.Text = "Save ";
            SaveButton.UseVisualStyleBackColor = false;
            SaveButton.Click += SaveButton_Click;
            // 
            // XRadio
            // 
            XRadio.AutoSize = true;
            XRadio.Checked = true;
            XRadio.Location = new Point(27, 479);
            XRadio.Name = "XRadio";
            XRadio.Size = new Size(44, 34);
            XRadio.TabIndex = 101;
            XRadio.TabStop = true;
            XRadio.Text = "X";
            XRadio.UseVisualStyleBackColor = true;
            XRadio.CheckedChanged += radioButton1_CheckedChanged;
            // 
            // ORadio
            // 
            ORadio.AutoSize = true;
            ORadio.Location = new Point(93, 479);
            ORadio.Name = "ORadio";
            ORadio.Size = new Size(48, 34);
            ORadio.TabIndex = 102;
            ORadio.Text = "O";
            ORadio.UseVisualStyleBackColor = true;
            // 
            // saveToDatasetCheckbox
            // 
            saveToDatasetCheckbox.AutoSize = true;
            saveToDatasetCheckbox.Checked = true;
            saveToDatasetCheckbox.CheckState = CheckState.Checked;
            saveToDatasetCheckbox.Font = new Font("Segoe UI", 14F);
            saveToDatasetCheckbox.Location = new Point(27, 520);
            saveToDatasetCheckbox.Name = "saveToDatasetCheckbox";
            saveToDatasetCheckbox.Size = new Size(158, 29);
            saveToDatasetCheckbox.TabIndex = 103;
            saveToDatasetCheckbox.Text = "Save to dataset";
            saveToDatasetCheckbox.UseVisualStyleBackColor = true;
            // 
            // ClearButton
            // 
            ClearButton.Cursor = Cursors.Hand;
            ClearButton.Font = new Font("Segoe UI", 12F);
            ClearButton.Location = new Point(110, 571);
            ClearButton.Name = "ClearButton";
            ClearButton.Size = new Size(78, 33);
            ClearButton.TabIndex = 104;
            ClearButton.Text = "Clear";
            ClearButton.UseVisualStyleBackColor = true;
            ClearButton.Click += ClearButton_Click;
            // 
            // DataSavedLabel
            // 
            DataSavedLabel.AutoSize = true;
            DataSavedLabel.Font = new Font("Segoe UI", 12F);
            DataSavedLabel.Location = new Point(168, 488);
            DataSavedLabel.Name = "DataSavedLabel";
            DataSavedLabel.Size = new Size(93, 21);
            DataSavedLabel.TabIndex = 105;
            DataSavedLabel.Text = "Data Saved!";
            DataSavedLabel.Visible = false;
            // 
            // NetworkComboBox
            // 
            NetworkComboBox.Font = new Font("Segoe UI", 12F);
            NetworkComboBox.FormattingEnabled = true;
            NetworkComboBox.Location = new Point(602, 55);
            NetworkComboBox.Name = "NetworkComboBox";
            NetworkComboBox.Size = new Size(271, 29);
            NetworkComboBox.TabIndex = 107;
            // 
            // SelectedNetworkLabel
            // 
            SelectedNetworkLabel.AutoSize = true;
            SelectedNetworkLabel.Font = new Font("Segoe UI", 12F);
            SelectedNetworkLabel.Location = new Point(480, 58);
            SelectedNetworkLabel.Name = "SelectedNetworkLabel";
            SelectedNetworkLabel.Size = new Size(118, 21);
            SelectedNetworkLabel.TabIndex = 106;
            SelectedNetworkLabel.Text = "Select Network:";
            // 
            // TrainButton
            // 
            TrainButton.Cursor = Cursors.Hand;
            TrainButton.Font = new Font("Segoe UI", 14F);
            TrainButton.Location = new Point(636, 110);
            TrainButton.Name = "TrainButton";
            TrainButton.Size = new Size(111, 41);
            TrainButton.TabIndex = 108;
            TrainButton.Text = "Train";
            TrainButton.UseVisualStyleBackColor = true;
            TrainButton.MouseClick += TrainButton_Click;
            // 
            // ClassifyButton
            // 
            ClassifyButton.Cursor = Cursors.Hand;
            ClassifyButton.Font = new Font("Segoe UI", 14F);
            ClassifyButton.Location = new Point(200, 571);
            ClassifyButton.Name = "ClassifyButton";
            ClassifyButton.Size = new Size(87, 33);
            ClassifyButton.TabIndex = 109;
            ClassifyButton.Text = "Classify";
            ClassifyButton.UseVisualStyleBackColor = true;
            ClassifyButton.Click += ClassifyButton_Click;
            // 
            // ConfusionMatrix
            // 
            ConfusionMatrix.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            ConfusionMatrix.BackgroundColor = SystemColors.Window;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 12F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Window;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            ConfusionMatrix.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            ConfusionMatrix.ColumnHeadersHeight = 25;
            ConfusionMatrix.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            ConfusionMatrix.Columns.AddRange(new DataGridViewColumn[] { X, O });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Window;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            ConfusionMatrix.DefaultCellStyle = dataGridViewCellStyle2;
            ConfusionMatrix.Location = new Point(480, 218);
            ConfusionMatrix.Name = "ConfusionMatrix";
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = SystemColors.Control;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 12F);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Window;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            ConfusionMatrix.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            ConfusionMatrix.RowHeadersWidth = 120;
            ConfusionMatrix.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            ConfusionMatrix.Size = new Size(320, 77);
            ConfusionMatrix.TabIndex = 110;
            // 
            // X
            // 
            X.Frozen = true;
            X.HeaderText = "X";
            X.Name = "X";
            X.ReadOnly = true;
            // 
            // O
            // 
            O.Frozen = true;
            O.HeaderText = "O";
            O.Name = "O";
            O.ReadOnly = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F);
            label1.Location = new Point(480, 180);
            label1.Name = "label1";
            label1.Size = new Size(132, 21);
            label1.TabIndex = 111;
            label1.Text = "Confusion Matrix:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F);
            label2.Location = new Point(480, 436);
            label2.Name = "label2";
            label2.Size = new Size(124, 21);
            label2.TabIndex = 112;
            label2.Text = "Macro Precision:";
            // 
            // MacroPrecisionValue
            // 
            MacroPrecisionValue.AutoSize = true;
            MacroPrecisionValue.Font = new Font("Segoe UI", 12F);
            MacroPrecisionValue.Location = new Point(610, 436);
            MacroPrecisionValue.Name = "MacroPrecisionValue";
            MacroPrecisionValue.Size = new Size(19, 21);
            MacroPrecisionValue.TabIndex = 113;
            MacroPrecisionValue.Text = "0";
            // 
            // MacroRecallValue
            // 
            MacroRecallValue.AutoSize = true;
            MacroRecallValue.Font = new Font("Segoe UI", 12F);
            MacroRecallValue.Location = new Point(610, 473);
            MacroRecallValue.Name = "MacroRecallValue";
            MacroRecallValue.Size = new Size(19, 21);
            MacroRecallValue.TabIndex = 115;
            MacroRecallValue.Text = "0";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F);
            label5.Location = new Point(480, 473);
            label5.Name = "label5";
            label5.Size = new Size(102, 21);
            label5.TabIndex = 114;
            label5.Text = "Macro Recall:";
            // 
            // MacroAccuracyValue
            // 
            MacroAccuracyValue.AutoSize = true;
            MacroAccuracyValue.Font = new Font("Segoe UI", 12F);
            MacroAccuracyValue.Location = new Point(610, 508);
            MacroAccuracyValue.Name = "MacroAccuracyValue";
            MacroAccuracyValue.Size = new Size(19, 21);
            MacroAccuracyValue.TabIndex = 117;
            MacroAccuracyValue.Text = "0";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 12F);
            label7.Location = new Point(480, 508);
            label7.Name = "label7";
            label7.Size = new Size(123, 21);
            label7.TabIndex = 116;
            label7.Text = "Macro Accuracy:";
            // 
            // MacroF1ScoreValue
            // 
            MacroF1ScoreValue.AutoSize = true;
            MacroF1ScoreValue.Font = new Font("Segoe UI", 12F);
            MacroF1ScoreValue.Location = new Point(610, 541);
            MacroF1ScoreValue.Name = "MacroF1ScoreValue";
            MacroF1ScoreValue.Size = new Size(19, 21);
            MacroF1ScoreValue.TabIndex = 119;
            MacroF1ScoreValue.Text = "0";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 12F);
            label9.Location = new Point(480, 541);
            label9.Name = "label9";
            label9.Size = new Size(121, 21);
            label9.TabIndex = 118;
            label9.Text = "Macro F1 Score:";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1010, 643);
            Controls.Add(MacroF1ScoreValue);
            Controls.Add(label9);
            Controls.Add(MacroAccuracyValue);
            Controls.Add(label7);
            Controls.Add(MacroRecallValue);
            Controls.Add(label5);
            Controls.Add(MacroPrecisionValue);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(ConfusionMatrix);
            Controls.Add(ClassifyButton);
            Controls.Add(TrainButton);
            Controls.Add(NetworkComboBox);
            Controls.Add(SelectedNetworkLabel);
            Controls.Add(DataSavedLabel);
            Controls.Add(ClearButton);
            Controls.Add(saveToDatasetCheckbox);
            Controls.Add(ORadio);
            Controls.Add(XRadio);
            Controls.Add(SaveButton);
            Controls.Add(appLabel);
            Font = new Font("Segoe UI", 16F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(5, 6, 5, 6);
            MaximizeBox = false;
            Name = "MainForm";
            Padding = new Padding(20);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Character Classification";
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)ConfusionMatrix).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label appLabel;
        private Button SaveButton;
        private RadioButton XRadio;
        private RadioButton ORadio;
        private CheckBox saveToDatasetCheckbox;
        private Button ClearButton;
        private Label DataSavedLabel;
        private ComboBox NetworkComboBox;
        private Label SelectedNetworkLabel;
        private Button TrainButton;
        private Button ClassifyButton;
        private DataGridView ConfusionMatrix;
        private DataGridViewTextBoxColumn X;
        private DataGridViewTextBoxColumn O;
        private Label label1;
        private Label label2;
        private Label MacroPrecisionValue;
        private Label MacroRecallValue;
        private Label label5;
        private Label MacroAccuracyValue;
        private Label label7;
        private Label MacroF1ScoreValue;
        private Label label9;
    }
}
