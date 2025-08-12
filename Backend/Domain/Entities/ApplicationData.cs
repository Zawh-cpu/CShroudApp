using System.Drawing;

namespace Backend.Domain.Entities;

public struct ApplicationData
{
    public required string Name { get; set; }
    public required string ExecutablePath { get; set; }
    public Icon? Icon { get; set; }
}