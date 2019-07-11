using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace MapOfModes
{
	class ClosingWarningForm : Form
	{
		public Button close { get; }
		public Button cancel { get; }

		public ClosingWarningForm()
		{
			this.Text = "�������������";
			this.Width = 400;
			this.Height = 150;
			this.ControlBox = false;
			this.FormBorderStyle = FormBorderStyle.FixedDialog;

			Label warningText = new Label
			{
				Location = new Point(15, 20),
				Size = new Size(385, 40),
				Font = new Font("Arial", 12, FontStyle.Regular),
				Text = "��������� ��� �� ��������� ������ �������, ����� ������ ����� ���� �������.",
			};

			this.cancel = new Button
			{
				Location = new Point(80, 70),
				Size = new Size(70, 24),
				Text = "������", 
			};

			this.close = new Button
			{
				Location = new Point(200, 70),
				Size = new Size(130, 24),
				Text = "�� ����� �������"
			};

			Controls.Add(warningText);
			Controls.Add(cancel);
			Controls.Add(close);
		}
	}
}