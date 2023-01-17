using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGlideable
{
    bool isGliding { get; set; }
    void Glide();
    void GlideRelease();
}
