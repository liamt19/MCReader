using System.Text;
using System.Xml.Linq;

namespace MCReader.NBT.Tags
{
    public class TAG_Compound : INBTTag
    {
        public int PayloadSize() => 0;
        private object _data;
        private string _name;
        public int TagID() => (int)TagType.TAG_Compound;


        public bool IsRoot = false;

        public void MakeRoot()
        {
            IsRoot = true;
            if (_name == null || _name.Length == 0)
            {
                _name = "root";
            }
        }

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
            _data = r.ReadCompound();

            if (IsRoot && _name?.Length == 0)
            {
                _name = "root";
            }
        }

        public TAG_Compound(object data = null)
        {
            this._data = data;
        }

        public string ToStringLevel(int level)
        {
            List<INBTTag> d = (List<INBTTag>)_data;

            StringBuilder sb = new StringBuilder();

            sb.Append('\t', level);

            if (IsRoot && _name.Length == 0)
            {
                sb.Append("root: [" + Environment.NewLine);
            }
            else if (_name != null)
            {
                sb.Append(_name + ": [" + Environment.NewLine);
            }
            else
            {
                sb.Append("Compound: [" + Environment.NewLine);
            }

            for (int i = 0; i < d.Count; i++)
            {
                if (d[i].TagID() != (int) TagType.TAG_Compound)
                {
                    sb.Append('\t', level + 1);
                }
                sb.Append(d[i].ToString() + ", " + Environment.NewLine);
            }
            sb.Remove(sb.Length - 2 - Environment.NewLine.Length, 2 + Environment.NewLine.Length);
            sb.Append('\t', level);
            sb.Append("]");
            return sb.ToString();
        }

        public override string ToString()
        {
            return ToStringLevel(0);
        }
    }
}
