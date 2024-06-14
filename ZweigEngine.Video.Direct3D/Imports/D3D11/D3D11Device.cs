using System.Runtime.InteropServices;
using ZweigEngine.Common.Platform;
using ZweigEngine.Common.Utility.Interop;
using ZweigEngine.Video.Direct3D.Imports.Constants;
using ZweigEngine.Video.Direct3D.Imports.D3D11.Constants;
using ZweigEngine.Video.Direct3D.Imports.D3D11.Structures;
using ZweigEngine.Video.Direct3D.Imports.DXGI;
using ZweigEngine.Video.Direct3D.Imports.Prototypes;
using ZweigEngine.Video.Direct3D.Imports.Structures;
using ZweigEngine.Video.Direct3D.Imports.VTables.D3D11;

namespace ZweigEngine.Video.Direct3D.Imports.D3D11;

internal sealed class D3D11Device : DXObject
{
	private delegate Win32HResult PfnCreateRenderTargetView(IntPtr self, IntPtr resource, IntPtr description, out IntPtr view);

	private readonly PfnCreateRenderTargetView m_createRenderTargetView;

	private D3D11Device(IntPtr pointer, Direct3DFeatureLevel featureLevel) : base(pointer)
	{
		FeatureLevel = featureLevel;
		LoadMethod((in D3D11DeviceMethodTable table) => table.CreateRenderTargetView, out m_createRenderTargetView);
	}

	public Direct3DFeatureLevel FeatureLevel { get; }

	public static bool TryCreate(PlatformLibraryLoader loader, DXGIAdapter? adapter, Direct3DDriverType driverType, out D3D11Device device, out D3D11DeviceContext context)
	{
		device  = null!;
		context = null!;

		loader.LoadFunction("d3d11", "D3D11CreateDevice", out PfnD3D11CreateDevice construct);
		if (construct(adapter?.Self ?? IntPtr.Zero,
		              (uint)driverType,
		              IntPtr.Zero,
		              (uint)D3D11CreateDeviceFlags.Singlethreaded,
		              IntPtr.Zero,
		              0u,
		              (uint)D3D11SdkVersion.Current,
		              out var devicePointer,
		              out var featureLevel,
		              out var contextPointer).Success)
		{
			device  = new D3D11Device(devicePointer, (Direct3DFeatureLevel)featureLevel);
			context = new D3D11DeviceContext(contextPointer);
			return true;
		}

		return false;
	}

	public bool CreateRenderTargetView(D3D11Texture2D resource, D3D11RenderTargetDescription? description, ref D3D11RenderTargetView? view)
	{
		if (description != null)
		{
			using var pinned = new PinnedObject<D3D11RenderTargetDescription>(description.Value, GCHandleType.Pinned);
			if (m_createRenderTargetView(Self, resource.Self, pinned.GetAddress(), out var pointer).Success)
			{
				view = new D3D11RenderTargetView(pointer);
				return true;
			}

			return false;
		}
		else
		{
			if (m_createRenderTargetView(Self, resource.Self, IntPtr.Zero, out var pointer).Success)
			{
				view = new D3D11RenderTargetView(pointer);
				return true;
			}

			return false;
		}
	}
}