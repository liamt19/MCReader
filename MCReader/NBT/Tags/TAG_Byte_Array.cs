using System.Text;

namespace MCReader.NBT.Tags
{
    public class TAG_Byte_Array : INBTTag
    {
        public int PayloadSize() => 1;
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
            _data = r.ReadByteArray();
        }

        public TAG_Byte_Array(object data = null)
        {
            this._data = data;
        }

        public override string ToString()
        {
            byte[] d = (byte[]) _data;
            StringBuilder sb = new StringBuilder();
            sb.Append(_name + ": [");
            for (int i = 0; i < d.Length; i++)
            {
                sb.AppendFormat("{0:X2}", d[i]);
            }
            sb.Remove(sb.Length - 2, 2);
            sb.Append("]");
            return sb.ToString();
        }
    }
}
