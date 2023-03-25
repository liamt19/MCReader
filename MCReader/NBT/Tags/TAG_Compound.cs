using System.Text;
using System.Xml.Linq;

namespace MCReader.NBT.Tags
{
    public class TAG_Compound : INBTTag
    {
        public int PayloadSize() => 0;
        private object _data;
        private string _name;

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

        public override string ToString()
        {
            List<INBTTag> d = (List<INBTTag>)_data;

            StringBuilder sb = new StringBuilder();

            if (IsRoot && _name.Length == 0)
            {
                sb.Append("root: [");
            }
            else
            {
                sb.Append(_name + ": [");
            }
            
            for (int i = 0; i < d.Count; i++)
            {
                sb.Append(d[i].ToString() + ", ");
            }
            sb.Remove(sb.Length - 2, 2);
            sb.Append("]");
            return sb.ToString();
        }
    }
}
