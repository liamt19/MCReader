namespace MCReader.NBT.Tags
{
    public class TAG_Double : INBTTag
    {
        public int PayloadSize() => 8;
        private object _data;
        private string _name;
        public int TagID() => (int)TagType.TAG_Double;

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
            _data = r.ReadDouble();
        }

        public TAG_Double(object data = null)
        {
            this._data = data;
        }

        public override string ToString()
        {
            return _name + ": " + _data.ToString();
        }
    }
}
