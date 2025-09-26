using UnityEngine;

[System.Serializable]
public class CollectedItensInfo
{
    [SerializeField] private string nameItem;
    [SerializeField] private int value;
    [SerializeField] private int qtd;


    // getters and setters of both fields
    public string NameItem { get => nameItem; set => nameItem = value; }
    public int Value { get => value; set => this.value = value; }
    public int Qtd { get => qtd; set => qtd = value; }


    public CollectedItensInfo(string nameItem, int value, int qtd)
    {
        this.nameItem = nameItem;
        this.value = value;
        this.qtd = qtd;
    }

    public override string ToString()
    {
        return "[Entity : CollectedItensInfo] : name => "+nameItem+ ", value => " +value+ ",  qtd => " +qtd;
    }
}