
using System;
using System.Collections.Generic;

public interface ICollectable
{
    Dictionary<string, int> returnMaterials { get; set; }
    void Collect();
    void Return();
}