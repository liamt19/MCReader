using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MCReaderForm
{
    public static class FormUtilities
    {
        //  https://stackoverflow.com/questions/53082492/export-list-of-treeview-to-text-file-in-c-sharp
        private static void TreeToString_Rec(TreeNode root, StringBuilder sb, int level)
        {
            sb.Append('\t', level);
            sb.Append(root.Text + Environment.NewLine);

            bool condense = false;

            if (root.Nodes.Count > 5)
            {
                condense = true;
                foreach (TreeNode childNode in root.Nodes)
                {
                    if (childNode.Nodes.Count != 0)
                    {
                        condense = false;
                        break;
                    }
                }
            }

            if (condense)
            {
                //  Condense lists into fewer lines.
                int perLine = 8;

                sb.Append('\t', level + 1);

                int j = 0;
                for (int i = 0; i < root.Nodes.Count; i++)
                {
                    TreeNode childNode = root.Nodes[i];
                    sb.Append(childNode.Text + ", ");
                    j++;
                    if (j == perLine || i == root.Nodes.Count - 1)
                    {
                        sb.Remove(sb.Length - 2, 2);
                        sb.Append(Environment.NewLine);

                        if (i != root.Nodes.Count - 1)
                        {
                            sb.Append('\t', level + 1);
                        }

                        j = 0;
                    }
                }
            }
            else
            {
                foreach (TreeNode childNode in root.Nodes)
                {
                    TreeToString_Rec(childNode, sb, level + 1);
                }
            }

        }

        public static string TreeToString(TreeNode root)
        {
            StringBuilder sb = new StringBuilder();
            TreeToString_Rec(root, sb, 0);
            return sb.ToString();
        }


        public static TreeNode EnumerateList(List<INBTTag> list)
        {
            TreeNode res = new TreeNode();
            for (int i = 0; i < list.Count; i++)
            {
                INBTTag tag = list[i];
                TreeNode temp;
                if ((tag is TAG_Compound) || (tag is TAG_List))
                {
                    List<INBTTag> tagList = ((List<INBTTag>)tag.Data());
                    temp = EnumerateList(tagList);
                    temp.Text = tag.Name();
                }
                else if ((tag is TAG_Int_Array) || (tag is TAG_Long_Array))
                {
                    INBTTag[] tagArr = ((INBTTag[])tag.Data());
                    temp = EnumerateList(tagArr.ToList());
                    temp.Text = tag.Name();
                }
                else
                {
                    temp = new TreeNode(tag.ToString());
                }

                int imgIdx = (int)INBTTag.IDOf(tag);
                temp.ImageIndex = imgIdx;
                temp.SelectedImageIndex = imgIdx;

                if (i == 0 && tag is TAG_Compound && (tag as TAG_Compound).IsRoot)
                {
                    res = temp;
                }
                else
                {
                    res.Nodes.Add(temp);
                }
            }

            return res;
        }

    }
}
