﻿using System.Runtime.InteropServices;

namespace ZweigEngine.Video.Direct3D.Imports.VTables.D3D11;

[Guid("c0bfa96c-e089-44fb-8eaf-26f8796190da")]
[StructLayout(LayoutKind.Sequential)]
internal struct D3D11DeviceContextMethodTable
{
	public D3D11DeviceChildMethodTable Super;
	public IntPtr                      VSSetConstantBuffers;
	public IntPtr                      PSSetShaderResources;
	public IntPtr                      PSSetShader;
	public IntPtr                      PSSetSamplers;
	public IntPtr                      VSSetShader;
	public IntPtr                      DrawIndexed;
	public IntPtr                      Draw;
	public IntPtr                      Map;
	public IntPtr                      Unmap;
	public IntPtr                      PSSetConstantBuffers;
	public IntPtr                      IASetInputLayout;
	public IntPtr                      IASetVertexBuffers;
	public IntPtr                      IASetIndexBuffer;
	public IntPtr                      DrawIndexedInstanced;
	public IntPtr                      DrawInstanced;
	public IntPtr                      GSSetConstantBuffers;
	public IntPtr                      GSSetShader;
	public IntPtr                      IASetPrimitiveTopology;
	public IntPtr                      VSSetShaderResources;
	public IntPtr                      VSSetSamplers;
	public IntPtr                      Begin;
	public IntPtr                      End;
	public IntPtr                      GetData;
	public IntPtr                      SetPredication;
	public IntPtr                      GSSetShaderResources;
	public IntPtr                      GSSetSamplers;
	public IntPtr                      OMSetRenderTargets;
	public IntPtr                      OMSetRenderTargetsAndUnorderedAccessViews;
	public IntPtr                      OMSetBlendState;
	public IntPtr                      OMSetDepthStencilState;
	public IntPtr                      SOSetTargets;
	public IntPtr                      DrawAuto;
	public IntPtr                      DrawIndexedInstancedIndirect;
	public IntPtr                      DrawInstancedIndirect;
	public IntPtr                      Dispatch;
	public IntPtr                      DispatchIndirect;
	public IntPtr                      RSSetState;
	public IntPtr                      RSSetViewports;
	public IntPtr                      RSSetScissorRects;
	public IntPtr                      CopySubresourceRegion;
	public IntPtr                      CopyResource;
	public IntPtr                      UpdateSubresource;
	public IntPtr                      CopyStructureCount;
	public IntPtr                      ClearRenderTargetView;
	public IntPtr                      ClearUnorderedAccessViewUint;
	public IntPtr                      ClearUnorderedAccessViewFloat;
	public IntPtr                      ClearDepthStencilView;
	public IntPtr                      GenerateMips;
	public IntPtr                      SetResourceMinLOD;
	public IntPtr                      GetResourceMinLOD;
	public IntPtr                      ResolveSubresource;
	public IntPtr                      ExecuteCommandList;
	public IntPtr                      HSSetShaderResources;
	public IntPtr                      HSSetShader;
	public IntPtr                      HSSetSamplers;
	public IntPtr                      HSSetConstantBuffers;
	public IntPtr                      DSSetShaderResources;
	public IntPtr                      DSSetShader;
	public IntPtr                      DSSetSamplers;
	public IntPtr                      DSSetConstantBuffers;
	public IntPtr                      CSSetShaderResources;
	public IntPtr                      CSSetUnorderedAccessViews;
	public IntPtr                      CSSetShader;
	public IntPtr                      CSSetSamplers;
	public IntPtr                      CSSetConstantBuffers;
	public IntPtr                      VSGetConstantBuffers;
	public IntPtr                      PSGetShaderResources;
	public IntPtr                      PSGetShader;
	public IntPtr                      PSGetSamplers;
	public IntPtr                      VSGetShader;
	public IntPtr                      PSGetConstantBuffers;
	public IntPtr                      IAGetInputLayout;
	public IntPtr                      IAGetVertexBuffers;
	public IntPtr                      IAGetIndexBuffer;
	public IntPtr                      GSGetConstantBuffers;
	public IntPtr                      GSGetShader;
	public IntPtr                      IAGetPrimitiveTopology;
	public IntPtr                      VSGetShaderResources;
	public IntPtr                      VSGetSamplers;
	public IntPtr                      GetPredication;
	public IntPtr                      GSGetShaderResources;
	public IntPtr                      GSGetSamplers;
	public IntPtr                      OMGetRenderTargets;
	public IntPtr                      OMGetRenderTargetsAndUnorderedAccessViews;
	public IntPtr                      OMGetBlendState;
	public IntPtr                      OMGetDepthStencilState;
	public IntPtr                      SOGetTargets;
	public IntPtr                      RSGetState;
	public IntPtr                      RSGetViewports;
	public IntPtr                      RSGetScissorRects;
	public IntPtr                      HSGetShaderResources;
	public IntPtr                      HSGetShader;
	public IntPtr                      HSGetSamplers;
	public IntPtr                      HSGetConstantBuffers;
	public IntPtr                      DSGetShaderResources;
	public IntPtr                      DSGetShader;
	public IntPtr                      DSGetSamplers;
	public IntPtr                      DSGetConstantBuffers;
	public IntPtr                      CSGetShaderResources;
	public IntPtr                      CSGetUnorderedAccessViews;
	public IntPtr                      CSGetShader;
	public IntPtr                      CSGetSamplers;
	public IntPtr                      CSGetConstantBuffers;
	public IntPtr                      ClearState;
	public IntPtr                      Flush;
	public IntPtr                      GetDeviceContextType;
	public IntPtr                      GetContextFlags;
	public IntPtr                      FinishCommandList;
}