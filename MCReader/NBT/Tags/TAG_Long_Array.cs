using System.Text;

namespace MCReader.NBT.Tags
{
    public class TAG_Long_Array : INBTTag
    {
        public int PayloadSize() => 8;
        private object _data;
        private string _name;
        public int TagID() => (int)TagType.TAG_Long_Array;
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
            _data = r.ReadLongArray();
        }

        public TAG_Long_Array(object data = null)
        {
            this._data = data;
        }

        public override string ToString()
        {
            TAG_Long[] d = (TAG_Long[])_data;

            StringBuilder sb = new StringBuilder();
            if (_name != null)
            {
                sb.Append(_name + ": [");
            }
            else
            {
                sb.Append("Long array: [");
            }
            for (int i = 0; i < d.Length; i++)
            {
                if (d[i].Name() != null)
                {
                    sb.Append(d[i].Name() + ": " + d[i].Data() + ", ");
                }
                else
                {
                    sb.Append(d[i].Data() + ", ");
                }
            }
            sb.Remove(sb.Length - 2, 2);
            sb.Append("]");
            return sb.ToString();
        }
    }
}
