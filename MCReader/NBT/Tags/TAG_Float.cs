﻿namespace MCReader.NBT.Tags
{
    public class TAG_Float : INBTTag
    {
        public int PayloadSize() => 4;
        private object _data;
        private string _name;
        public int TagID() => (int)TagType.TAG_Float;

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
            _data = r.ReadFloat();
        }

        public TAG_Float(object data = null)
        {
            this._data = data;
        }

        public override string ToString()
        {
            return _name + ": " + _data.ToString();
        }
    }
}
