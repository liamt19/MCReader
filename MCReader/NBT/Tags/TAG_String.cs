namespace MCReader.NBT.Tags
{
    public class TAG_String : INBTTag
    {
        public int PayloadSize() => 0;
        private object _data;
        private string _name;
        public int TagID() => (int)TagType.TAG_String;

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
            _data = r.ReadString();
        }

        public TAG_String(object data = null)
        {
            this._data = data;
        }

        public override string ToString()
        {
            return _name + ": \"" + _data.ToString() + "\"";

            if (_name != null && _name.Length > 0)
            {
                return _name + ": \"" + _data.ToString() + "\"";
            }
            else
            {
                return ": \"" + _data.ToString() + "\"";
            }
        }
    }
}
