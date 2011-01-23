//
// Copyright (c) 2002 Rui Godinho Lopes <ruilopes.com>
// All rights reserved.
//
// This source file(s) may be redistributed by any means PROVIDING they
// are not sold for profit without the authors expressed written consent,
// and providing that this notice and the authors name and all copyright
// notices remain intact.
//
// It would be nice, but not necessary, that you acknowledge the author 
// in your application "About Box" or Documentation.
//
// An email letting me know that you are using it would be nice as well.
// That's not much to ask considering the amount of work that went into
// this.
//
// THIS SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED. USE IT AT YOUT OWN RISK. THE AUTHOR ACCEPTS NO
// LIABILITY FOR ANY DATA DAMAGE/LOSS THAT THIS PRODUCT MAY CAUSE.
//

//
// History
// -------
//
// 27 feb 2002
// + The class ClipboardViewer in now in a separate file,
//   Rgl.Components.CliboardViewer.cs and in a different namespace,
//   Rgl.Components.
// + ClipboardViewer class now correctly creates itself on Win9X/ME.
//
//	14 feb 2002
// + first version.
//

using System;
using System.Drawing;
using System.Windows.Forms;


///<para>The application dialog box.</para>
class MyForm : Form {

	public MyForm() {
		InitializeComponent();

		Font = new Font("tahoma", 8);
		Text = "Drag&Drop and Clipboard formats viewer";
		SizeGripStyle = SizeGripStyle.Show;
		StartPosition = FormStartPosition.CenterScreen;

		AllowDrop = true; // Because we want to be a drop target!
	}

	///<para>Constructs and initializes all child controls of this dialog box.</para>
	private void InitializeComponent() {

		ClientSize = new Size(450, 260);


		PictureBox pictureBox = new PictureBox();
		pictureBox.Location = new System.Drawing.Point(4, 8);
		pictureBox.Anchor = AnchorStyles.Left | AnchorStyles.Top;
		pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
		pictureBox.Image = Image.FromFile("i.gif");
		Controls.Add(pictureBox);


		Label label1 = new Label();
		label1.AutoSize = true;
		label1.Location = new System.Drawing.Point(22, 8);
		label1.Anchor = AnchorStyles.Left | AnchorStyles.Top;
		label1.Text = "Drag something in this form or copy something to clipboard and see all the data formats";
		Controls.Add(label1);


		tree = new TreeView();
		tree.Location = new Point(4, 28);
		tree.Size = new Size(442, 208);
		tree.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

		TreeNode rootNode = new TreeNode("Object in Clipboard");
			tree.Nodes.Add(rootNode);
			rootNode.Nodes.Add(clipboardNativeNode = new TreeNode("Native formats"));
			rootNode.Nodes.Add(clipboardConvertNode = new TreeNode("Convert formats"));
		rootNode = new TreeNode("Object from Drag&Drop");
			tree.Nodes.Add(rootNode);
			rootNode.Nodes.Add(dragDropNativeNode = new TreeNode("Native formats"));
			rootNode.Nodes.Add(dragDropConvertNode = new TreeNode("Convert formats"));

			dragDropNativeNode.Nodes.Add("[ no object dropped ]");
			dragDropConvertNode.Nodes.Add("[ no object dropped ]");

		Controls.Add(tree);

		// Label with two links to me! :)
		LinkLabel link = new LinkLabel();
 		link.Location = new System.Drawing.Point(4, 240);
		link.Size = new System.Drawing.Size(250, 80);
		link.AutoSize = true;
		link.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
		link.Text = "by Rui Lopes";
		link.Links.Add(3, 9,  "http://www.ruilopes.com");
 		link.LinkClicked += new LinkLabelLinkClickedEventHandler(Link_LinkClicked);
 		Controls.Add(link);

		// creates and initializes the clipboard viewer

		viewer = new Rgl.Components.ClipboardViewer();
		viewer.ClipboardChanged += new Rgl.Components.VoidEventHandler(Viewer_ClipboardChanged);
		viewer.Enabled = true; // Enable clipboard viewing
	}

	///<para>Frees resources.</para>
	protected override void Dispose(bool disposing) {
		if (disposing)
			viewer.Dispose();
		base.Dispose(disposing);
	}

	///<para>Print all available drop formats.</para>
	protected override void OnDragEnter(DragEventArgs e) {
		PopulateTreeNodes(dragDropNativeNode, dragDropConvertNode, e.Data);
		base.OnDragEnter(e);
	}

	/// <summary>Extracts all format types from IDataObject and add's them to
	/// the given tree nodes.</summary>
	private void PopulateTreeNodes(TreeNode nativeNode, TreeNode convertNode, IDataObject data) {
		tree.BeginUpdate();
		try {
			nativeNode.Nodes.Clear();
			convertNode.Nodes.Clear();

			string[] nativeFormats = data.GetFormats(false);
			foreach (string format in nativeFormats)
				nativeNode.Nodes.Add(format);

			foreach (string format in data.GetFormats(true))
				if (Array.IndexOf(nativeFormats, format) < 0)
					convertNode.Nodes.Add(format);
		} finally {
			tree.EndUpdate();
		}
	}

	///<para>Updates the Clipoard nodes when the Clipboard content change.</para>
	private void Viewer_ClipboardChanged() {
		PopulateTreeNodes(clipboardNativeNode, clipboardConvertNode, Clipboard.GetDataObject());
	}

	///<para>Start the computer browser in the specified uri.</para>
	private void Link_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
		e.Link.Visited = true;
		using (System.Diagnostics.Process.Start(e.Link.LinkData.ToString())) {}
	}

	private Rgl.Components.ClipboardViewer viewer;
	private TreeView tree;
	private TreeNode clipboardNativeNode;
	private TreeNode clipboardConvertNode;
	private TreeNode dragDropNativeNode;
	private TreeNode dragDropConvertNode;
}



// Our Great Application!
class TheApp {
	[STAThread]
	public static void Main() {
		Application.Run(new MyForm());
	}
}