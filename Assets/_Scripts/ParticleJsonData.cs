using System;
using System.Collections.Generic;

[Serializable]
public class ParticleJsonData
{
    public string name = "ParticalInfo";

    public List<ParticleCat> ParticalDetails = new List<ParticleCat>();
}
