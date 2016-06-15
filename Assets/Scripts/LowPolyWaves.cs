using UnityEngine;
using System.Collections;

public class LowPolyWaves : MonoBehaviour {

	Vector3 waveSource1 = new Vector3 (2.0f, 0.0f, 2.0f);
	public float freq1 = 0.1f;
	public float amp1 = 0.01f;
	public float waveLength1 = 0.05f;
	public int testy = 0;
	Mesh mesh;
	Vector3[] vertices;

	// Use this for initialization
	void Start () {
		MeshFilter mf = GetComponent<MeshFilter>();
		if(mf == null)
		{
			Debug.Log("No mesh filter");
			return;
		}
		mesh = mf.mesh;
		vertices = mesh.vertices;
	}

	// Update is called once per frame
	void Update () {
		CalcWave();
	}

	void CalcWave()
	{
		for(int i = 0; i < vertices.Length; i++)
		{
			Vector3 v = vertices[i];
			v.y = 0.0f;
			float dist = Vector3.Distance(v, waveSource1);
			//Debug.Log ("dist1 " + dist);
			dist = (dist % waveLength1) / waveLength1;
			//Debug.Log ("dist2" + dist);
			v.y = amp1 * Mathf.Sin(Time.time * Mathf.PI * 2.0f * freq1
				+ (Mathf.PI * 2.0f * dist));
            //Debug.Log ("y" + v.y);
            vertices[i] = v;
            /*
			if (((int)(Random.value * 100)) % 20 == 0) {
				v.y = testy % 5;
				vertices [i] = v;
			}
            */
            testy++;
		}
		mesh.vertices = vertices;
	}
}