using System;
using System.Collections;
using System.Collections.Generic;
using TerrainGenerator.InfiniteTerrain;
using UnityEngine;

public class ProsseduralInfiniteLandscape : MonoBehaviour {
  public Material material;
  public ProcGenChunk landscapePrefab;

  [Header("Geometry")] public int xResolution = 20;
  public int zResolution = 20;
  public float meshScale = 1;
  public float yScale = 1;

  [Header("Noise")] [Range(1, 9)] public int octaves;
  public float lacunarity = 2f;
  [Range(0f, 1f)] public float gain = 0.5f;
  public float perlinScale = 1;

  public float uvScale = 0.1f;

  private void Awake() {
    var topLeft = CreateTerrainChunk(new Vector2(0, 2 * zResolution));
    var topMid = CreateTerrainChunk(new Vector2(xResolution, 2 * zResolution));
    var topRight = CreateTerrainChunk(new Vector2(2 * xResolution, 2 * zResolution));

    var midLeft = CreateTerrainChunk(new Vector2(0, zResolution));
    var midMid = CreateTerrainChunk(new Vector2(xResolution, zResolution));
    var midRight = CreateTerrainChunk(new Vector2(2 * xResolution, zResolution));

    var bottomLeft = CreateTerrainChunk(new Vector2(0, 0));
    var bottomMid = CreateTerrainChunk(new Vector2(xResolution, 0));
    var bottomRight = CreateTerrainChunk(new Vector2(2 * xResolution, 0));
  }

  private ProcGenChunk CreateTerrainChunk(Vector2 position) {
    var chunk = Instantiate(landscapePrefab);
    chunk.InitInfiniteLandscapeGenerator(material, xResolution, zResolution, meshScale, yScale, octaves, lacunarity,
      gain, perlinScale, uvScale, position);
    chunk.transform.SetParent(transform);

    return chunk;
  }
}