namespace Godot;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public partial class Node3D
{
    public override void SetActive(bool active)
    {
        base.SetActive(active);

        if (active)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
}

