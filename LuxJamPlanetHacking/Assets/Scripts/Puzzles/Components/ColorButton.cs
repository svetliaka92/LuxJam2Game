using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorButton : BaseComponent
{
    public enum Direction
    {
        up,
        right,
        down,
        left
    }

    public enum Type
    {
        row,
        column
    }

    [SerializeField] private Button button;
    [SerializeField] private Direction direction;
    [SerializeField] private Type type;
    [SerializeField] private int id;

    public Button GetButton => button;

    public Direction ButtonDirection => direction;
    public Type ButtonType => type;
    public int ID => id;

    public override void Init(Puzzle parent)
    {
        base.Init(parent);

        button.onClick.AddListener(OnComponentInteract);
    }

    protected override void OnComponentInteract()
    {
        base.OnComponentInteract();

        _parent.OnComponentInteract(this);
    }
}
