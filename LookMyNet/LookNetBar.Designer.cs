namespace LookMyNet
{
    partial class LookNetBar
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AdapterList = new System.Windows.Forms.ListBox();
            this.Timepiece = new System.Windows.Forms.Timer(this.components);
            this.textBox_Down = new System.Windows.Forms.TextBox();
            this.textBox_Up = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // AdapterList
            // 
            this.AdapterList.FormattingEnabled = true;
            this.AdapterList.ItemHeight = 12;
            this.AdapterList.Location = new System.Drawing.Point(12, 2);
            this.AdapterList.Name = "AdapterList";
            this.AdapterList.Size = new System.Drawing.Size(260, 40);
            this.AdapterList.TabIndex = 0;
            this.AdapterList.SelectedIndexChanged += new System.EventHandler(this.AdepterList_SelectedIndexChanged);
            // 
            // Timepiece
            // 
            this.Timepiece.Tick += new System.EventHandler(this.Timepiece_Tick);
            // 
            // textBox_Down
            // 
            this.textBox_Down.Location = new System.Drawing.Point(33, 60);
            this.textBox_Down.Name = "textBox_Down";
            this.textBox_Down.Size = new System.Drawing.Size(100, 21);
            this.textBox_Down.TabIndex = 1;
            // 
            // textBox_Up
            // 
            this.textBox_Up.Location = new System.Drawing.Point(33, 102);
            this.textBox_Up.Name = "textBox_Up";
            this.textBox_Up.Size = new System.Drawing.Size(100, 21);
            this.textBox_Up.TabIndex = 2;
            // 
            // LookNetBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PaleTurquoise;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.ControlBox = false;
            this.Controls.Add(this.textBox_Up);
            this.Controls.Add(this.textBox_Down);
            this.Controls.Add(this.AdapterList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LookNetBar";
            this.Opacity = 0.8D;
            this.ShowIcon = false;
            this.Text = "Form1";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.LookNetBar_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LookNetBar_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LookNetBar_MouseMove);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox AdapterList;
        private System.Windows.Forms.Timer Timepiece;
        private System.Windows.Forms.TextBox textBox_Down;
        private System.Windows.Forms.TextBox textBox_Up;
    }
}

