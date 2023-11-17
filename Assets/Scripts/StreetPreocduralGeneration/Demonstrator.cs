using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demonstrator : MonoBehaviour
{
    public LSystemGenerator lSystem;
    public List<Vector3> positions = new List<Vector3>();
    public GameObject prefab;
    public Material lineMaterial;

    private int _length = 8;
    private float angle = 90;

    public int length
    {
        get
        {
            if (_length > 0)
            {
                return _length;
            }
            else
            {
                return 1;
            }
        }
        set => _length = value;
    }

    private void Start()
    {
        var sequence = lSystem.GenerateSentence();
        Visualization(sequence);
    }

    private void Visualization(string sequence)
    {
        Stack<Parameters> parameters = new Stack<Parameters>();
        var currentPosition = Vector3.zero;

        Vector3 direction = Vector3.forward;
        Vector3 tempPosition = Vector3.zero;

        positions.Add(currentPosition);

        foreach (var letter in sequence)
        {
            Encode encode = (Encode)letter;
            switch (encode)
            {
                case Encode.save:
                    parameters.Push(new Parameters
                    {
                        position = currentPosition,
                        direction = direction,
                        length = length
                    });
                    break;
                case Encode.load:
                    if (parameters.Count > 0)
                    {
                        var Paremeter = parameters.Pop();
                        currentPosition = Paremeter.position;
                        direction = Paremeter.direction;
                        length = Paremeter.length;
                    }
                    else
                    {
                        throw new System.Exception("No Saved Points");
                    }
                    break;
                case Encode.draw:
                    tempPosition = currentPosition;
                    currentPosition += direction * length;
                    DrawLine(tempPosition, currentPosition, Color.red);
                    length -= 2;
                    positions.Add(currentPosition);
                    break;
                case Encode.turnRight:
                    direction = Quaternion.AngleAxis(angle, Vector3.up) * direction;
                    break;
                case Encode.turnLeft:
                    direction = Quaternion.AngleAxis(-angle, Vector3.up) * direction;
                    break;
                default:
                    break;

            }
        }
        foreach (var position in positions)
        {
            Instantiate(prefab, position, Quaternion.identity);
        }
    }

    private void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        GameObject line = new GameObject("line");
        line.transform.position = start;
        var lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    public enum Encode
    {
        unknown = '1',
        save = '[',
        load = ']',
        draw = 'F',
        turnRight = '+',
        turnLeft = '-',
    }
}

