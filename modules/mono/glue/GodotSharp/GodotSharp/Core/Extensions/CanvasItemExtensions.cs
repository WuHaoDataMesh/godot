namespace Godot;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public partial class CanvasItem
{
    public override void SetActive(bool active)
    {
        base.SetActive(active);

        this.Visible = active;
    }
}
