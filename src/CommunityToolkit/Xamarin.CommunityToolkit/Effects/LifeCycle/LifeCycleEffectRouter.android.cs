﻿using System;
using System.Linq;
using Xamarin.CommunityToolkit.Android.Effects;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;

[assembly: ExportEffect(typeof(LifeCycleEffectRouter), nameof(LifecycleEffect))]

namespace Xamarin.CommunityToolkit.Android.Effects
{
	public class LifeCycleEffectRouter : PlatformEffect
	{
		View nativeView;
		LifecycleEffect lifeCycleEffect;

		protected override void OnAttached()
		{
			lifeCycleEffect = Element.Effects.OfType<LifecycleEffect>().FirstOrDefault() ??
				throw new ArgumentNullException($"The effect {nameof(LifecycleEffect)} can't be null.");

			nativeView = Control ?? Container;

			nativeView.ViewAttachedToWindow += OnNativeViewViewAttachedToWindow;
			nativeView.ViewDetachedFromWindow += OnNativeViewViewDetachedFromWindow;
		}

		void OnNativeViewViewAttachedToWindow(object sender, View.ViewAttachedToWindowEventArgs e) => lifeCycleEffect.RaiseLoadedEvent(Element);

		void OnNativeViewViewDetachedFromWindow(object sender, View.ViewDetachedFromWindowEventArgs e)
		{
			lifeCycleEffect.RaiseUnloadedEvent(Element);
			nativeView.ViewDetachedFromWindow -= OnNativeViewViewDetachedFromWindow;
			nativeView.ViewAttachedToWindow -= OnNativeViewViewAttachedToWindow;
			nativeView = null;
			lifeCycleEffect = null;
		}

		protected override void OnDetached()
		{
		}
	}
}
