using System.Reflection.Emit;
using System.Text;

namespace MCReader.NBT.Tags
{
    public class TAG_List : INBTTag
    {
        public int PayloadSize() => 0;
        private object _data;
        private string _name;
        public int TagID() => (int)TagType.TAG_List;

        public object Data()
        {
            return _data;
        }

        public string Name()
        {
            return _name;
        }

        public void Read(NBTReader r, bool noName)
        {
            if (!noName)
                _name = r.ReadString();
            _data = r.ReadList();
        }

        public TAG_List(object data = null)
        {
            this._data = data;
        }

        public string ToStringLevel(int level)
        {
            List<INBTTag> d = (List<INBTTag>)_data;

            StringBuilder sb = new StringBuilder();
            if (_name != null)
            {
                sb.Append(_name + ": [");
            }
            else
            {
                sb.Append("List: [");
            }

            if (d.Count > 0 && d[0].TagID() == (int)TagType.TAG_List)
            {
                sb.Append(Environment.NewLine);
                sb.Append('\t', level);
                for (int i = 0; i < d.Count; i++)
                {
                    sb.Append(((TAG_List)d[i]).ToStringLevel(level + 1) + ", ");
                }
            }
            else if (d.Count > 0 && d[0].TagID() == (int)TagType.TAG_Compound)
            {
                sb.Append(Environment.NewLine);
                sb.Append('\t', level);
                for (int i = 0; i < d.Count; i++)
                {
                    sb.Append(((TAG_Compound)d[i]).ToString() + ", ");
                }
            }
            else
            {
                for (int i = 0; i < d.Count; i++)
                {
                    if (d[i].TagID() == (int)TagType.TAG_Int_Array)
                    {
                        sb.Append(((TAG_Int_Array)d[i]).ToString());
                    }
                    else if (d[i].TagID() == (int)TagType.TAG_Long_Array)
                    {
                        sb.Append(((TAG_Long_Array)d[i]).ToString());
                    }
                    else if (d[i].Name() != null)
                    {
                        sb.Append(d[i].Name() + ": " + d[i].Data().ToString() + ", ");
                    }
                    else
                    {
                        sb.Append(d[i].Data().ToString() + ", ");
                    }
                }
            }

            if (d.Count > 0)
            {
                sb.Remove(sb.Length - 2, 2);
            }
            sb.Append("]");
            return sb.ToString();
        }

        public override string ToString()
        {
            return ToStringLevel(0);
        }
    }
}
