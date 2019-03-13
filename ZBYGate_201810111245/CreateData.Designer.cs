namespace ZBYGate_Data_Collection
{
    partial class CreateData
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("节点1");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("节点2");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("节点3");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("节点4");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("节点0", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4});
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Items.AddRange(new object[] {
            "本地用户表",
            "运行数据表",
            "出闸数据表",
            "流量数据表"});
            this.checkedListBox1.Location = new System.Drawing.Point(690, 12);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(94, 324);
            this.checkedListBox1.TabIndex = 5;
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(12, 12);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "节点1";
            treeNode1.Text = "节点1";
            treeNode2.Name = "节点2";
            treeNode2.Text = "节点2";
            treeNode3.Name = "节点3";
            treeNode3.Text = "节点3";
            treeNode4.Name = "节点4";
            treeNode4.Text = "节点4";
            treeNode5.Name = "节点0";
            treeNode5.Text = "节点0";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode5});
            this.treeView1.Size = new System.Drawing.Size(121, 97);
            this.treeView1.TabIndex = 6;
            // 
            // CreateData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 348);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.checkedListBox1);
            this.Name = "CreateData";
            this.Text = "CreateData";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.TreeView treeView1;
    }
}