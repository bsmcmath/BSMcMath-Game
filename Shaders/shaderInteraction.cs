using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class shaderInteraction : MonoBehaviour
{
    public ComputeBuffer interactions;
    public interaction[] interactionArray;
    public int interactionCount;
    [HideInInspector] public int interactionsID, interactionCountID;

    public RenderTexture interactionTexture;
    [HideInInspector] public RenderTexture copyTexture;
    public ComputeShader textureCompute;
    [HideInInspector] public int interactionTexID, computeCopyID, kernelFillID, kernelShiftRelaxID, kernelApplyInteractionsID;

    public Vector3 interactionExtents;
    [HideInInspector] public Vector3 interactionCenter;
    [HideInInspector] public int shiftXID, shiftYID, shiftZID;
    [HideInInspector] public int extentsMinID, extentsMaxID;
    [HideInInspector] public Vector3 shiftDistance;
    [HideInInspector] public Vector3 lodCenter;
    [HideInInspector] public int lodCenterID;

    public Transform player;
    public shaderInteractor[] interactors;
    public struct interaction
    {
        public Vector3 pos;
        public float size;
    }

    void Awake()
    {
        //Array and Buffer
        interactions = new ComputeBuffer(64, Marshal.SizeOf(typeof(interaction)));
        interactionArray = new interaction[64];
        interactionCount = 0;
        interactors = new shaderInteractor[64];
        interactionsID = Shader.PropertyToID("interactions");
        interactionCountID = Shader.PropertyToID("interactionCount");
        Shader.SetGlobalBuffer(interactionsID, interactions);
        Shader.SetGlobalInteger(interactionCountID, interactionCount);

        //Textures
        interactionTexture = new RenderTexture(interactionTexture);
        interactionTexture.enableRandomWrite = true;
        copyTexture = new RenderTexture(interactionTexture);
        copyTexture.enableRandomWrite = true;
        interactionTexID = Shader.PropertyToID("interactionTex");
        if (!interactionTexture.IsCreated()) interactionTexture.Create();
        if (!copyTexture.IsCreated()) copyTexture.Create();
        Shader.SetGlobalTexture(interactionTexID, interactionTexture);
        computeCopyID = Shader.PropertyToID("copyTex");

        //Compute Kernels
        kernelFillID = textureCompute.FindKernel("fill");
        kernelShiftRelaxID = textureCompute.FindKernel("shiftRelax");
        kernelApplyInteractionsID = textureCompute.FindKernel("applyInteractions");
        textureCompute.SetTexture(kernelFillID, interactionTexID, interactionTexture);
        textureCompute.SetTexture(kernelFillID, interactionTexID, interactionTexture);
        textureCompute.SetTexture(kernelShiftRelaxID, interactionTexID, interactionTexture);
        textureCompute.SetTexture(kernelShiftRelaxID, computeCopyID, copyTexture);
        textureCompute.SetTexture(kernelApplyInteractionsID, interactionTexID, interactionTexture);
        textureCompute.SetTexture(kernelApplyInteractionsID, computeCopyID, copyTexture);
        textureCompute.Dispatch(kernelFillID, 64, 64, 1);
        textureCompute.SetBuffer(kernelApplyInteractionsID, interactionsID, interactions);

        //Interaction Space
        interactionCenter = Main.main.camera.transform.position;
        extentsMinID = Shader.PropertyToID("extentsMin");
        extentsMaxID = Shader.PropertyToID("extentsMax");
        shiftXID = Shader.PropertyToID("shiftX");
        shiftYID = Shader.PropertyToID("shiftY");
        shiftZID = Shader.PropertyToID("shiftZ");
        shiftDistance = interactionExtents * 100 * 2 / 2048;
        lodCenter = Main.main.camera.transform.position;
        lodCenterID = Shader.PropertyToID("lodCenter");
    }
    public void add(shaderInteractor i)
    {
        interactors[interactionCount] = i;
        i.index = interactionCount;
        interactionCount++;
    }
    public void remove(shaderInteractor i)
    {
        interactionCount--;
        interactors[i.index] = interactors[interactionCount];
        interactors[i.index].index = i.index;
    }
    private void FixedUpdate()
    {
        //Array and Buffer
        for (int i = 0; i < interactionCount; i++)
        {
            interactionArray[i].pos = interactors[i].transform.position;
            interactionArray[i].size = interactors[i].size;
        }
        interactions.SetData(interactionArray, 0, 0, interactionCount);
        Shader.SetGlobalInteger(interactionCountID, interactionCount);
        textureCompute.SetInt(interactionCountID, interactionCount);

        //Texture Extents
        TerrainCasts.stepRay(Main.main.camera.transform.position, Main.main.camera.transform.rotation * Vector3.forward, 100, out terrainHit hit);
        if (hit.position.x > interactionCenter.x + shiftDistance.x)
        {
            textureCompute.SetInt(shiftXID, 100);
            interactionCenter.x += shiftDistance.x;
        }
        else if (hit.position.x < interactionCenter.x - shiftDistance.x)
        {
            textureCompute.SetInt(shiftXID, -100);
            interactionCenter.x -= shiftDistance.x;
        }
        else
        {
            textureCompute.SetInt(shiftXID, 0);
        }

        if (hit.position.y > interactionCenter.y + shiftDistance.y)
        {
            textureCompute.SetFloat(shiftYID, shiftDistance.y / (2 * interactionExtents.y));
            interactionCenter.y += shiftDistance.y;
        }
        else if (hit.position.y < interactionCenter.y - shiftDistance.y)
        {
            textureCompute.SetFloat(shiftYID, -shiftDistance.y / (2 * interactionExtents.y));
            interactionCenter.y -= shiftDistance.y;
        }
        else
        {
            textureCompute.SetFloat(shiftYID, 0);
        }

        if (hit.position.z > interactionCenter.z + shiftDistance.z)
        {
            textureCompute.SetInt(shiftZID, 100);
            interactionCenter.z += shiftDistance.z;
        }
        else if (hit.position.z < interactionCenter.z - shiftDistance.z)
        {
            textureCompute.SetInt(shiftZID, -100);
            interactionCenter.z -= shiftDistance.z;
        }
        else
        {
            textureCompute.SetInt(shiftZID, 0);
        }

        Shader.SetGlobalVector(extentsMinID, interactionCenter - interactionExtents);
        Shader.SetGlobalVector(extentsMaxID, interactionCenter + interactionExtents);
        textureCompute.SetVector(extentsMinID, interactionCenter - interactionExtents);
        textureCompute.SetVector(extentsMaxID, interactionCenter + interactionExtents);

        if (player) lodCenter = player.position;
        else if (hit.hit) lodCenter = hit.position;
        else lodCenter = Main.main.camera.transform.position;
        Shader.SetGlobalVector(lodCenterID, lodCenter);

        textureCompute.Dispatch(kernelShiftRelaxID, 64, 64, 1);
        textureCompute.Dispatch(kernelApplyInteractionsID, 64, 64, 1);
    }
    private void OnDisable()
    {
        interactions.Release();
        Shader.SetGlobalInteger(interactionCountID, 0);
    }
    private void OnGUI()
    {
        //GUI.DrawTexture(new Rect(0, Screen.currentResolution.height - 200, 200, 200), interactionTexture, ScaleMode.StretchToFill, false, 10);
    }
}

