using System.Text;

namespace MCReader.NBT.Tags
{
    public class TAG_List : INBTTag
    {
        public int PayloadSize() => 0;
        private object _data;
        private string _name;

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

        public override string ToString()
        {
            List<INBTTag> d = (List<INBTTag>)_data;

            StringBuilder sb = new StringBuilder();
            sb.Append(_name + ": [");
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
