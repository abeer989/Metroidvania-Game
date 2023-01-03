using System;

[Serializable]
public class SFXData
{
    public SFXData(int _sfxIndex, bool _adj = false)
    {
        SFXIndex = _sfxIndex;
        adjust = _adj;
    }

    public int SFXIndex;
    public bool adjust;
}
