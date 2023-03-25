namespace MCReader.NBT.Tags
{
    public class TAG_Byte : INBTTag
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
            _data = r.ReadSByte();
        }

        public TAG_Byte(object data = null)
        {
            this._data = data;
        }

        public override string ToString()
        {
            return _name + ": " + _data.ToString();
        }
    }
}
