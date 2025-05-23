; ModuleID = 'obj\Debug\130\android\marshal_methods.x86_64.ll'
source_filename = "obj\Debug\130\android\marshal_methods.x86_64.ll"
target datalayout = "e-m:e-p270:32:32-p271:32:32-p272:64:64-i64:64-f80:128-n8:16:32:64-S128"
target triple = "x86_64-unknown-linux-android"


%struct.MonoImage = type opaque

%struct.MonoClass = type opaque

%struct.MarshalMethodsManagedClass = type {
	i32,; uint32_t token
	%struct.MonoClass*; MonoClass* klass
}

%struct.MarshalMethodName = type {
	i64,; uint64_t id
	i8*; char* name
}

%class._JNIEnv = type opaque

%class._jobject = type {
	i8; uint8_t b
}

%class._jclass = type {
	i8; uint8_t b
}

%class._jstring = type {
	i8; uint8_t b
}

%class._jthrowable = type {
	i8; uint8_t b
}

%class._jarray = type {
	i8; uint8_t b
}

%class._jobjectArray = type {
	i8; uint8_t b
}

%class._jbooleanArray = type {
	i8; uint8_t b
}

%class._jbyteArray = type {
	i8; uint8_t b
}

%class._jcharArray = type {
	i8; uint8_t b
}

%class._jshortArray = type {
	i8; uint8_t b
}

%class._jintArray = type {
	i8; uint8_t b
}

%class._jlongArray = type {
	i8; uint8_t b
}

%class._jfloatArray = type {
	i8; uint8_t b
}

%class._jdoubleArray = type {
	i8; uint8_t b
}

; assembly_image_cache
@assembly_image_cache = local_unnamed_addr global [0 x %struct.MonoImage*] zeroinitializer, align 8
; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = local_unnamed_addr constant [236 x i64] [
	i64 24362543149721218, ; 0: Xamarin.AndroidX.DynamicAnimation => 0x568d9a9a43a682 => 56
	i64 120698629574877762, ; 1: Mono.Android => 0x1accec39cafe242 => 11
	i64 210515253464952879, ; 2: Xamarin.AndroidX.Collection.dll => 0x2ebe681f694702f => 45
	i64 232391251801502327, ; 3: Xamarin.AndroidX.SavedState.dll => 0x3399e9cbc897277 => 83
	i64 263803244706540312, ; 4: Rg.Plugins.Popup.dll => 0x3a937a743542b18 => 14
	i64 295915112840604065, ; 5: Xamarin.AndroidX.SlidingPaneLayout => 0x41b4d3a3088a9a1 => 84
	i64 316157742385208084, ; 6: Xamarin.AndroidX.Core.Core.Ktx.dll => 0x46337caa7dc1b14 => 50
	i64 634308326490598313, ; 7: Xamarin.AndroidX.Lifecycle.Runtime.dll => 0x8cd840fee8b6ba9 => 68
	i64 663835873230324403, ; 8: DarkFit app => 0x9366b35a50e02b3 => 5
	i64 702024105029695270, ; 9: System.Drawing.Common => 0x9be17343c0e7726 => 2
	i64 720058930071658100, ; 10: Xamarin.AndroidX.Legacy.Support.Core.UI => 0x9fe29c82844de74 => 61
	i64 872800313462103108, ; 11: Xamarin.AndroidX.DrawerLayout => 0xc1ccf42c3c21c44 => 55
	i64 940822596282819491, ; 12: System.Transactions => 0xd0e792aa81923a3 => 109
	i64 996343623809489702, ; 13: Xamarin.Forms.Platform => 0xdd3b93f3b63db26 => 97
	i64 1000557547492888992, ; 14: Mono.Security.dll => 0xde2b1c9cba651a0 => 116
	i64 1120440138749646132, ; 15: Xamarin.Google.Android.Material.dll => 0xf8c9a5eae431534 => 99
	i64 1202444765235964449, ; 16: Syncfusion.Expander.XForms.dll => 0x10aff124a5eaea21 => 18
	i64 1315114680217950157, ; 17: Xamarin.AndroidX.Arch.Core.Common.dll => 0x124039d5794ad7cd => 40
	i64 1425944114962822056, ; 18: System.Runtime.Serialization.dll => 0x13c9f89e19eaf3a8 => 4
	i64 1624659445732251991, ; 19: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0x168bf32877da9957 => 38
	i64 1628611045998245443, ; 20: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 0x1699fd1e1a00b643 => 70
	i64 1636321030536304333, ; 21: Xamarin.AndroidX.Legacy.Support.Core.Utils.dll => 0x16b5614ec39e16cd => 62
	i64 1743969030606105336, ; 22: System.Memory.dll => 0x1833d297e88f2af8 => 25
	i64 1795316252682057001, ; 23: Xamarin.AndroidX.AppCompat.dll => 0x18ea3e9eac997529 => 39
	i64 1836611346387731153, ; 24: Xamarin.AndroidX.SavedState => 0x197cf449ebe482d1 => 83
	i64 1865037103900624886, ; 25: Microsoft.Bcl.AsyncInterfaces => 0x19e1f15d56eb87f6 => 8
	i64 1875917498431009007, ; 26: Xamarin.AndroidX.Annotation.dll => 0x1a08990699eb70ef => 36
	i64 1981742497975770890, ; 27: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x1b80904d5c241f0a => 69
	i64 2040001226662520565, ; 28: System.Threading.Tasks.Extensions.dll => 0x1c4f8a4ea894a6f5 => 117
	i64 2064708342624596306, ; 29: Xamarin.Kotlin.StdLib.Jdk7.dll => 0x1ca7514c5eecb152 => 104
	i64 2076847052340733488, ; 30: Syncfusion.Core.XForms => 0x1cd27163f7962630 => 16
	i64 2136356949452311481, ; 31: Xamarin.AndroidX.MultiDex.dll => 0x1da5dd539d8acbb9 => 74
	i64 2165725771938924357, ; 32: Xamarin.AndroidX.Browser => 0x1e0e341d75540745 => 43
	i64 2262844636196693701, ; 33: Xamarin.AndroidX.DrawerLayout.dll => 0x1f673d352266e6c5 => 55
	i64 2284400282711631002, ; 34: System.Web.Services => 0x1fb3d1f42fd4249a => 114
	i64 2304837677853103545, ; 35: Xamarin.AndroidX.ResourceInspection.Annotation.dll => 0x1ffc6da80d5ed5b9 => 82
	i64 2329709569556905518, ; 36: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x2054ca829b447e2e => 65
	i64 2335503487726329082, ; 37: System.Text.Encodings.Web => 0x2069600c4d9d1cfa => 30
	i64 2337758774805907496, ; 38: System.Runtime.CompilerServices.Unsafe => 0x207163383edbc828 => 28
	i64 2469392061734276978, ; 39: Syncfusion.Core.XForms.Android.dll => 0x22450aff2ad74f72 => 15
	i64 2470498323731680442, ; 40: Xamarin.AndroidX.CoordinatorLayout => 0x2248f922dc398cba => 49
	i64 2479423007379663237, ; 41: Xamarin.AndroidX.VectorDrawable.Animated.dll => 0x2268ae16b2cba985 => 89
	i64 2497223385847772520, ; 42: System.Runtime => 0x22a7eb7046413568 => 29
	i64 2547086958574651984, ; 43: Xamarin.AndroidX.Activity.dll => 0x2359121801df4a50 => 35
	i64 2592350477072141967, ; 44: System.Xml.dll => 0x23f9e10627330e8f => 33
	i64 2624866290265602282, ; 45: mscorlib.dll => 0x246d65fbde2db8ea => 12
	i64 2694427813909235223, ; 46: Xamarin.AndroidX.Preference.dll => 0x256487d230fe0617 => 78
	i64 2783046991838674048, ; 47: System.Runtime.CompilerServices.Unsafe.dll => 0x269f5e7e6dc37c80 => 28
	i64 2787234703088983483, ; 48: Xamarin.AndroidX.Startup.StartupRuntime => 0x26ae3f31ef429dbb => 85
	i64 2960931600190307745, ; 49: Xamarin.Forms.Core => 0x2917579a49927da1 => 95
	i64 3017704767998173186, ; 50: Xamarin.Google.Android.Material => 0x29e10a7f7d88a002 => 99
	i64 3289520064315143713, ; 51: Xamarin.AndroidX.Lifecycle.Common => 0x2da6b911e3063621 => 64
	i64 3303437397778967116, ; 52: Xamarin.AndroidX.Annotation.Experimental => 0x2dd82acf985b2a4c => 37
	i64 3311221304742556517, ; 53: System.Numerics.Vectors.dll => 0x2df3d23ba9e2b365 => 27
	i64 3344514922410554693, ; 54: Xamarin.KotlinX.Coroutines.Core.Jvm => 0x2e6a1a9a18463545 => 107
	i64 3493805808809882663, ; 55: Xamarin.AndroidX.Tracing.Tracing.dll => 0x307c7ddf444f3427 => 87
	i64 3522470458906976663, ; 56: Xamarin.AndroidX.SwipeRefreshLayout => 0x30e2543832f52197 => 86
	i64 3531994851595924923, ; 57: System.Numerics => 0x31042a9aade235bb => 26
	i64 3571415421602489686, ; 58: System.Runtime.dll => 0x319037675df7e556 => 29
	i64 3716579019761409177, ; 59: netstandard.dll => 0x3393f0ed5c8c5c99 => 1
	i64 3727469159507183293, ; 60: Xamarin.AndroidX.RecyclerView => 0x33baa1739ba646bd => 81
	i64 3772598417116884899, ; 61: Xamarin.AndroidX.DynamicAnimation.dll => 0x345af645b473efa3 => 56
	i64 3966267475168208030, ; 62: System.Memory => 0x370b03412596249e => 25
	i64 4201423742386704971, ; 63: Xamarin.AndroidX.Core.Core.Ktx => 0x3a4e74a233da124b => 50
	i64 4525561845656915374, ; 64: System.ServiceModel.Internals => 0x3ece06856b710dae => 115
	i64 4636684751163556186, ; 65: Xamarin.AndroidX.VersionedParcelable.dll => 0x4058d0370893015a => 91
	i64 4759461199762736555, ; 66: Xamarin.AndroidX.Lifecycle.Process.dll => 0x420d00be961cc5ab => 67
	i64 4782108999019072045, ; 67: Xamarin.AndroidX.AsyncLayoutInflater.dll => 0x425d76cc43bb0a2d => 42
	i64 4794310189461587505, ; 68: Xamarin.AndroidX.Activity => 0x4288cfb749e4c631 => 35
	i64 4795410492532947900, ; 69: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0x428cb86f8f9b7bbc => 86
	i64 5142919913060024034, ; 70: Xamarin.Forms.Platform.Android.dll => 0x475f52699e39bee2 => 96
	i64 5203618020066742981, ; 71: Xamarin.Essentials => 0x4836f704f0e652c5 => 94
	i64 5205316157927637098, ; 72: Xamarin.AndroidX.LocalBroadcastManager => 0x483cff7778e0c06a => 72
	i64 5348796042099802469, ; 73: Xamarin.AndroidX.Media => 0x4a3abda9415fc165 => 73
	i64 5376510917114486089, ; 74: Xamarin.AndroidX.VectorDrawable.Animated => 0x4a9d3431719e5d49 => 89
	i64 5408338804355907810, ; 75: Xamarin.AndroidX.Transition => 0x4b0e477cea9840e2 => 88
	i64 5451019430259338467, ; 76: Xamarin.AndroidX.ConstraintLayout.dll => 0x4ba5e94a845c2ce3 => 48
	i64 5507995362134886206, ; 77: System.Core.dll => 0x4c705499688c873e => 22
	i64 5692067934154308417, ; 78: Xamarin.AndroidX.ViewPager2.dll => 0x4efe49a0d4a8bb41 => 93
	i64 5757522595884336624, ; 79: Xamarin.AndroidX.Concurrent.Futures.dll => 0x4fe6d44bd9f885f0 => 46
	i64 5814345312393086621, ; 80: Xamarin.AndroidX.Preference => 0x50b0b44182a5c69d => 78
	i64 5896680224035167651, ; 81: Xamarin.AndroidX.Lifecycle.LiveData.dll => 0x51d5376bfbafdda3 => 66
	i64 6085203216496545422, ; 82: Xamarin.Forms.Platform.dll => 0x5472fc15a9574e8e => 97
	i64 6086316965293125504, ; 83: FormsViewGroup.dll => 0x5476f10882baef80 => 6
	i64 6222399776351216807, ; 84: System.Text.Json.dll => 0x565a67a0ffe264a7 => 31
	i64 6319713645133255417, ; 85: Xamarin.AndroidX.Lifecycle.Runtime => 0x57b42213b45b52f9 => 68
	i64 6401687960814735282, ; 86: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0x58d75d486341cfb2 => 65
	i64 6504860066809920875, ; 87: Xamarin.AndroidX.Browser.dll => 0x5a45e7c43bd43d6b => 43
	i64 6548213210057960872, ; 88: Xamarin.AndroidX.CustomView.dll => 0x5adfed387b066da8 => 53
	i64 6591024623626361694, ; 89: System.Web.Services.dll => 0x5b7805f9751a1b5e => 114
	i64 6659513131007730089, ; 90: Xamarin.AndroidX.Legacy.Support.Core.UI.dll => 0x5c6b57e8b6c3e1a9 => 61
	i64 6876862101832370452, ; 91: System.Xml.Linq => 0x5f6f85a57d108914 => 34
	i64 6894844156784520562, ; 92: System.Numerics.Vectors => 0x5faf683aead1ad72 => 27
	i64 7026608356547306326, ; 93: Syncfusion.Core.XForms.dll => 0x618387125bcb2356 => 16
	i64 7036436454368433159, ; 94: Xamarin.AndroidX.Legacy.Support.V4.dll => 0x61a671acb33d5407 => 63
	i64 7103753931438454322, ; 95: Xamarin.AndroidX.Interpolator.dll => 0x62959a90372c7632 => 60
	i64 7488575175965059935, ; 96: System.Xml.Linq.dll => 0x67ecc3724534ab5f => 34
	i64 7635363394907363464, ; 97: Xamarin.Forms.Core.dll => 0x69f6428dc4795888 => 95
	i64 7637365915383206639, ; 98: Xamarin.Essentials.dll => 0x69fd5fd5e61792ef => 94
	i64 7654504624184590948, ; 99: System.Net.Http => 0x6a3a4366801b8264 => 3
	i64 7735352534559001595, ; 100: Xamarin.Kotlin.StdLib.dll => 0x6b597e2582ce8bfb => 103
	i64 7820441508502274321, ; 101: System.Data => 0x6c87ca1e14ff8111 => 108
	i64 7836164640616011524, ; 102: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x6cbfa6390d64d704 => 38
	i64 7998806787784973739, ; 103: DarkFit app.Android => 0x6f017864dce81dab => 0
	i64 8044118961405839122, ; 104: System.ComponentModel.Composition => 0x6fa2739369944712 => 113
	i64 8083354569033831015, ; 105: Xamarin.AndroidX.Lifecycle.Common.dll => 0x702dd82730cad267 => 64
	i64 8087206902342787202, ; 106: System.Diagnostics.DiagnosticSource => 0x703b87d46f3aa082 => 23
	i64 8103644804370223335, ; 107: System.Data.DataSetExtensions.dll => 0x7075ee03be6d50e7 => 110
	i64 8167236081217502503, ; 108: Java.Interop.dll => 0x7157d9f1a9b8fd27 => 7
	i64 8187640529827139739, ; 109: Xamarin.KotlinX.Coroutines.Android => 0x71a057ae90f0109b => 106
	i64 8398329775253868912, ; 110: Xamarin.AndroidX.ConstraintLayout.Core.dll => 0x748cdc6f3097d170 => 47
	i64 8400357532724379117, ; 111: Xamarin.AndroidX.Navigation.UI.dll => 0x749410ab44503ded => 77
	i64 8426919725312979251, ; 112: Xamarin.AndroidX.Lifecycle.Process => 0x74f26ed7aa033133 => 67
	i64 8518412311883997971, ; 113: System.Collections.Immutable => 0x76377add7c28e313 => 21
	i64 8598790081731763592, ; 114: Xamarin.AndroidX.Emoji2.ViewsHelper.dll => 0x77550a055fc61d88 => 58
	i64 8601935802264776013, ; 115: Xamarin.AndroidX.Transition.dll => 0x7760370982b4ed4d => 88
	i64 8626175481042262068, ; 116: Java.Interop => 0x77b654e585b55834 => 7
	i64 8639588376636138208, ; 117: Xamarin.AndroidX.Navigation.Runtime => 0x77e5fbdaa2fda2e0 => 76
	i64 8684531736582871431, ; 118: System.IO.Compression.FileSystem => 0x7885a79a0fa0d987 => 112
	i64 8725526185868997716, ; 119: System.Diagnostics.DiagnosticSource.dll => 0x79174bd613173454 => 23
	i64 8853378295825400934, ; 120: Xamarin.Kotlin.StdLib.Common.dll => 0x7add84a720d38466 => 102
	i64 8951477988056063522, ; 121: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller => 0x7c3a09cd9ccf5e22 => 80
	i64 9312692141327339315, ; 122: Xamarin.AndroidX.ViewPager2 => 0x813d54296a634f33 => 93
	i64 9324707631942237306, ; 123: Xamarin.AndroidX.AppCompat => 0x8168042fd44a7c7a => 39
	i64 9662334977499516867, ; 124: System.Numerics.dll => 0x8617827802b0cfc3 => 26
	i64 9678050649315576968, ; 125: Xamarin.AndroidX.CoordinatorLayout.dll => 0x864f57c9feb18c88 => 49
	i64 9711637524876806384, ; 126: Xamarin.AndroidX.Media.dll => 0x86c6aadfd9a2c8f0 => 73
	i64 9808709177481450983, ; 127: Mono.Android.dll => 0x881f890734e555e7 => 11
	i64 9825649861376906464, ; 128: Xamarin.AndroidX.Concurrent.Futures => 0x885bb87d8abc94e0 => 46
	i64 9834056768316610435, ; 129: System.Transactions.dll => 0x8879968718899783 => 109
	i64 9907349773706910547, ; 130: Xamarin.AndroidX.Emoji2.ViewsHelper => 0x897dfa20b758db53 => 58
	i64 9998632235833408227, ; 131: Mono.Security => 0x8ac2470b209ebae3 => 116
	i64 10038780035334861115, ; 132: System.Net.Http.dll => 0x8b50e941206af13b => 3
	i64 10226222362177979215, ; 133: Xamarin.Kotlin.StdLib.Jdk7 => 0x8dead70ebbc6434f => 104
	i64 10229024438826829339, ; 134: Xamarin.AndroidX.CustomView => 0x8df4cb880b10061b => 53
	i64 10321854143672141184, ; 135: Xamarin.Jetbrains.Annotations.dll => 0x8f3e97a7f8f8c580 => 101
	i64 10376576884623852283, ; 136: Xamarin.AndroidX.Tracing.Tracing => 0x900101b2f888c2fb => 87
	i64 10406448008575299332, ; 137: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 0x906b2153fcb3af04 => 107
	i64 10430153318873392755, ; 138: Xamarin.AndroidX.Core => 0x90bf592ea44f6673 => 51
	i64 10447083246144586668, ; 139: Microsoft.Bcl.AsyncInterfaces.dll => 0x90fb7edc816203ac => 8
	i64 10847732767863316357, ; 140: Xamarin.AndroidX.Arch.Core.Common => 0x968ae37a86db9f85 => 40
	i64 11002576679268595294, ; 141: Microsoft.Extensions.Logging.Abstractions => 0x98b1013215cd365e => 10
	i64 11023048688141570732, ; 142: System.Core => 0x98f9bc61168392ac => 22
	i64 11030442608075099922, ; 143: DarkFit app.dll => 0x9914011c39a64f12 => 5
	i64 11037814507248023548, ; 144: System.Xml => 0x992e31d0412bf7fc => 33
	i64 11162124722117608902, ; 145: Xamarin.AndroidX.ViewPager => 0x9ae7d54b986d05c6 => 92
	i64 11340910727871153756, ; 146: Xamarin.AndroidX.CursorAdapter => 0x9d630238642d465c => 52
	i64 11392833485892708388, ; 147: Xamarin.AndroidX.Print.dll => 0x9e1b79b18fcf6824 => 79
	i64 11529969570048099689, ; 148: Xamarin.AndroidX.ViewPager.dll => 0xa002ae3c4dc7c569 => 92
	i64 11578238080964724296, ; 149: Xamarin.AndroidX.Legacy.Support.V4 => 0xa0ae2a30c4cd8648 => 63
	i64 11580057168383206117, ; 150: Xamarin.AndroidX.Annotation => 0xa0b4a0a4103262e5 => 36
	i64 11591352189662810718, ; 151: Xamarin.AndroidX.Startup.StartupRuntime.dll => 0xa0dcc167234c525e => 85
	i64 11597940890313164233, ; 152: netstandard => 0xa0f429ca8d1805c9 => 1
	i64 11672361001936329215, ; 153: Xamarin.AndroidX.Interpolator => 0xa1fc8e7d0a8999ff => 60
	i64 12102847907131387746, ; 154: System.Buffers => 0xa7f5f40c43256f62 => 20
	i64 12137774235383566651, ; 155: Xamarin.AndroidX.VectorDrawable => 0xa872095bbfed113b => 90
	i64 12145679461940342714, ; 156: System.Text.Json => 0xa88e1f1ebcb62fba => 31
	i64 12269460666702402136, ; 157: System.Collections.Immutable.dll => 0xaa45e178506c9258 => 21
	i64 12451044538927396471, ; 158: Xamarin.AndroidX.Fragment.dll => 0xaccaff0a2955b677 => 59
	i64 12466513435562512481, ; 159: Xamarin.AndroidX.Loader.dll => 0xad01f3eb52569061 => 71
	i64 12487638416075308985, ; 160: Xamarin.AndroidX.DocumentFile.dll => 0xad4d00fa21b0bfb9 => 54
	i64 12538491095302438457, ; 161: Xamarin.AndroidX.CardView.dll => 0xae01ab382ae67e39 => 44
	i64 12550732019250633519, ; 162: System.IO.Compression => 0xae2d28465e8e1b2f => 111
	i64 12700543734426720211, ; 163: Xamarin.AndroidX.Collection => 0xb041653c70d157d3 => 45
	i64 12828192437253469131, ; 164: Xamarin.Kotlin.StdLib.Jdk8.dll => 0xb206e50e14d873cb => 105
	i64 12963446364377008305, ; 165: System.Drawing.Common.dll => 0xb3e769c8fd8548b1 => 2
	i64 13129914918964716986, ; 166: Xamarin.AndroidX.Emoji2.dll => 0xb636d40db3fe65ba => 57
	i64 13370592475155966277, ; 167: System.Runtime.Serialization => 0xb98de304062ea945 => 4
	i64 13401370062847626945, ; 168: Xamarin.AndroidX.VectorDrawable.dll => 0xb9fb3b1193964ec1 => 90
	i64 13404347523447273790, ; 169: Xamarin.AndroidX.ConstraintLayout.Core => 0xba05cf0da4f6393e => 47
	i64 13454009404024712428, ; 170: Xamarin.Google.Guava.ListenableFuture => 0xbab63e4543a86cec => 100
	i64 13465488254036897740, ; 171: Xamarin.Kotlin.StdLib => 0xbadf06394d106fcc => 103
	i64 13491513212026656886, ; 172: Xamarin.AndroidX.Arch.Core.Runtime.dll => 0xbb3b7bc905569876 => 41
	i64 13572454107664307259, ; 173: Xamarin.AndroidX.RecyclerView.dll => 0xbc5b0b19d99f543b => 81
	i64 13622732128915678507, ; 174: Syncfusion.Core.XForms.Android => 0xbd0daab1e651e52b => 15
	i64 13647894001087880694, ; 175: System.Data.dll => 0xbd670f48cb071df6 => 108
	i64 13764593499834068841, ; 176: Npgsql => 0xbf05a8dc3342af69 => 13
	i64 13828521679616088467, ; 177: Xamarin.Kotlin.StdLib.Common => 0xbfe8c733724e1993 => 102
	i64 13849821837823696015, ; 178: Syncfusion.Expander.XForms.Android => 0xc0347394fdeddc8f => 17
	i64 13959074834287824816, ; 179: Xamarin.AndroidX.Fragment => 0xc1b8989a7ad20fb0 => 59
	i64 13967638549803255703, ; 180: Xamarin.Forms.Platform.Android => 0xc1d70541e0134797 => 96
	i64 13970307180132182141, ; 181: Syncfusion.Licensing => 0xc1e0805ccade287d => 19
	i64 14124974489674258913, ; 182: Xamarin.AndroidX.CardView => 0xc405fd76067d19e1 => 44
	i64 14172845254133543601, ; 183: Xamarin.AndroidX.MultiDex => 0xc4b00faaed35f2b1 => 74
	i64 14261073672896646636, ; 184: Xamarin.AndroidX.Print => 0xc5e982f274ae0dec => 79
	i64 14486659737292545672, ; 185: Xamarin.AndroidX.Lifecycle.LiveData => 0xc90af44707469e88 => 66
	i64 14495724990987328804, ; 186: Xamarin.AndroidX.ResourceInspection.Annotation => 0xc92b2913e18d5d24 => 82
	i64 14536303476482288245, ; 187: Syncfusion.Expander.XForms => 0xc9bb52fec700aa75 => 18
	i64 14538127318538747197, ; 188: Syncfusion.Licensing.dll => 0xc9c1cdc518e77d3d => 19
	i64 14551742072151931844, ; 189: System.Text.Encodings.Web.dll => 0xc9f22c50f1b8fbc4 => 30
	i64 14644440854989303794, ; 190: Xamarin.AndroidX.LocalBroadcastManager.dll => 0xcb3b815e37daeff2 => 72
	i64 14792063746108907174, ; 191: Xamarin.Google.Guava.ListenableFuture.dll => 0xcd47f79af9c15ea6 => 100
	i64 14852515768018889994, ; 192: Xamarin.AndroidX.CursorAdapter.dll => 0xce1ebc6625a76d0a => 52
	i64 14954917835170835695, ; 193: Microsoft.Extensions.DependencyInjection.Abstractions.dll => 0xcf8a8a895a82ecef => 9
	i64 14987728460634540364, ; 194: System.IO.Compression.dll => 0xcfff1ba06622494c => 111
	i64 14988210264188246988, ; 195: Xamarin.AndroidX.DocumentFile => 0xd000d1d307cddbcc => 54
	i64 15150743910298169673, ; 196: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller.dll => 0xd2424150783c3149 => 80
	i64 15279429628684179188, ; 197: Xamarin.KotlinX.Coroutines.Android.dll => 0xd40b704b1c4c96f4 => 106
	i64 15370334346939861994, ; 198: Xamarin.AndroidX.Core.dll => 0xd54e65a72c560bea => 51
	i64 15391712275433856905, ; 199: Microsoft.Extensions.DependencyInjection.Abstractions => 0xd59a58c406411f89 => 9
	i64 15582737692548360875, ; 200: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 0xd841015ed86f6aab => 70
	i64 15609085926864131306, ; 201: System.dll => 0xd89e9cf3334914ea => 24
	i64 15777549416145007739, ; 202: Xamarin.AndroidX.SlidingPaneLayout.dll => 0xdaf51d99d77eb47b => 84
	i64 15810740023422282496, ; 203: Xamarin.Forms.Xaml => 0xdb6b08484c22eb00 => 98
	i64 15847085070278954535, ; 204: System.Threading.Channels.dll => 0xdbec27e8f35f8e27 => 32
	i64 15963349826457351533, ; 205: System.Threading.Tasks.Extensions => 0xdd893616f748b56d => 117
	i64 16106398470792018018, ; 206: Syncfusion.Expander.XForms.Android.dll => 0xdf856c12e673f862 => 17
	i64 16154507427712707110, ; 207: System => 0xe03056ea4e39aa26 => 24
	i64 16321164108206115771, ; 208: Microsoft.Extensions.Logging.Abstractions.dll => 0xe2806c487e7b0bbb => 10
	i64 16391261437891890046, ; 209: Npgsql.dll => 0xe379756ee21acb7e => 13
	i64 16423015068819898779, ; 210: Xamarin.Kotlin.StdLib.Jdk8 => 0xe3ea453135e5c19b => 105
	i64 16565028646146589191, ; 211: System.ComponentModel.Composition.dll => 0xe5e2cdc9d3bcc207 => 113
	i64 16621146507174665210, ; 212: Xamarin.AndroidX.ConstraintLayout => 0xe6aa2caf87dedbfa => 48
	i64 16677317093839702854, ; 213: Xamarin.AndroidX.Navigation.UI => 0xe771bb8960dd8b46 => 77
	i64 16822611501064131242, ; 214: System.Data.DataSetExtensions => 0xe975ec07bb5412aa => 110
	i64 16833383113903931215, ; 215: mscorlib => 0xe99c30c1484d7f4f => 12
	i64 17024911836938395553, ; 216: Xamarin.AndroidX.Annotation.Experimental.dll => 0xec44a31d250e5fa1 => 37
	i64 17031351772568316411, ; 217: Xamarin.AndroidX.Navigation.Common.dll => 0xec5b843380a769fb => 75
	i64 17037200463775726619, ; 218: Xamarin.AndroidX.Legacy.Support.Core.Utils => 0xec704b8e0a78fc1b => 62
	i64 17118171214553292978, ; 219: System.Threading.Channels => 0xed8ff6060fc420b2 => 32
	i64 17285063141349522879, ; 220: Rg.Plugins.Popup => 0xefe0e158cc55fdbf => 14
	i64 17544493274320527064, ; 221: Xamarin.AndroidX.AsyncLayoutInflater => 0xf37a8fada41aded8 => 42
	i64 17704177640604968747, ; 222: Xamarin.AndroidX.Loader => 0xf5b1dfc36cac272b => 71
	i64 17710060891934109755, ; 223: Xamarin.AndroidX.Lifecycle.ViewModel => 0xf5c6c68c9e45303b => 69
	i64 17820498385379254854, ; 224: DarkFit app.Android.dll => 0xf74f20dd4cf3e246 => 0
	i64 17838668724098252521, ; 225: System.Buffers.dll => 0xf78faeb0f5bf3ee9 => 20
	i64 17882897186074144999, ; 226: FormsViewGroup => 0xf82cd03e3ac830e7 => 6
	i64 17891337867145587222, ; 227: Xamarin.Jetbrains.Annotations => 0xf84accff6fb52a16 => 101
	i64 17892495832318972303, ; 228: Xamarin.Forms.Xaml.dll => 0xf84eea293687918f => 98
	i64 17928294245072900555, ; 229: System.IO.Compression.FileSystem.dll => 0xf8ce18a0b24011cb => 112
	i64 18116111925905154859, ; 230: Xamarin.AndroidX.Arch.Core.Runtime => 0xfb695bd036cb632b => 41
	i64 18121036031235206392, ; 231: Xamarin.AndroidX.Navigation.Common => 0xfb7ada42d3d42cf8 => 75
	i64 18129453464017766560, ; 232: System.ServiceModel.Internals.dll => 0xfb98c1df1ec108a0 => 115
	i64 18260797123374478311, ; 233: Xamarin.AndroidX.Emoji2 => 0xfd6b623bde35f3e7 => 57
	i64 18305135509493619199, ; 234: Xamarin.AndroidX.Navigation.Runtime.dll => 0xfe08e7c2d8c199ff => 76
	i64 18380184030268848184 ; 235: Xamarin.AndroidX.VersionedParcelable => 0xff1387fe3e7b7838 => 91
], align 16
@assembly_image_cache_indices = local_unnamed_addr constant [236 x i32] [
	i32 56, i32 11, i32 45, i32 83, i32 14, i32 84, i32 50, i32 68, ; 0..7
	i32 5, i32 2, i32 61, i32 55, i32 109, i32 97, i32 116, i32 99, ; 8..15
	i32 18, i32 40, i32 4, i32 38, i32 70, i32 62, i32 25, i32 39, ; 16..23
	i32 83, i32 8, i32 36, i32 69, i32 117, i32 104, i32 16, i32 74, ; 24..31
	i32 43, i32 55, i32 114, i32 82, i32 65, i32 30, i32 28, i32 15, ; 32..39
	i32 49, i32 89, i32 29, i32 35, i32 33, i32 12, i32 78, i32 28, ; 40..47
	i32 85, i32 95, i32 99, i32 64, i32 37, i32 27, i32 107, i32 87, ; 48..55
	i32 86, i32 26, i32 29, i32 1, i32 81, i32 56, i32 25, i32 50, ; 56..63
	i32 115, i32 91, i32 67, i32 42, i32 35, i32 86, i32 96, i32 94, ; 64..71
	i32 72, i32 73, i32 89, i32 88, i32 48, i32 22, i32 93, i32 46, ; 72..79
	i32 78, i32 66, i32 97, i32 6, i32 31, i32 68, i32 65, i32 43, ; 80..87
	i32 53, i32 114, i32 61, i32 34, i32 27, i32 16, i32 63, i32 60, ; 88..95
	i32 34, i32 95, i32 94, i32 3, i32 103, i32 108, i32 38, i32 0, ; 96..103
	i32 113, i32 64, i32 23, i32 110, i32 7, i32 106, i32 47, i32 77, ; 104..111
	i32 67, i32 21, i32 58, i32 88, i32 7, i32 76, i32 112, i32 23, ; 112..119
	i32 102, i32 80, i32 93, i32 39, i32 26, i32 49, i32 73, i32 11, ; 120..127
	i32 46, i32 109, i32 58, i32 116, i32 3, i32 104, i32 53, i32 101, ; 128..135
	i32 87, i32 107, i32 51, i32 8, i32 40, i32 10, i32 22, i32 5, ; 136..143
	i32 33, i32 92, i32 52, i32 79, i32 92, i32 63, i32 36, i32 85, ; 144..151
	i32 1, i32 60, i32 20, i32 90, i32 31, i32 21, i32 59, i32 71, ; 152..159
	i32 54, i32 44, i32 111, i32 45, i32 105, i32 2, i32 57, i32 4, ; 160..167
	i32 90, i32 47, i32 100, i32 103, i32 41, i32 81, i32 15, i32 108, ; 168..175
	i32 13, i32 102, i32 17, i32 59, i32 96, i32 19, i32 44, i32 74, ; 176..183
	i32 79, i32 66, i32 82, i32 18, i32 19, i32 30, i32 72, i32 100, ; 184..191
	i32 52, i32 9, i32 111, i32 54, i32 80, i32 106, i32 51, i32 9, ; 192..199
	i32 70, i32 24, i32 84, i32 98, i32 32, i32 117, i32 17, i32 24, ; 200..207
	i32 10, i32 13, i32 105, i32 113, i32 48, i32 77, i32 110, i32 12, ; 208..215
	i32 37, i32 75, i32 62, i32 32, i32 14, i32 42, i32 71, i32 69, ; 216..223
	i32 0, i32 20, i32 6, i32 101, i32 98, i32 112, i32 41, i32 75, ; 224..231
	i32 115, i32 57, i32 76, i32 91 ; 232..235
], align 16

@marshal_methods_number_of_classes = local_unnamed_addr constant i32 0, align 4

; marshal_methods_class_cache
@marshal_methods_class_cache = global [0 x %struct.MarshalMethodsManagedClass] [
], align 8; end of 'marshal_methods_class_cache' array


@get_function_pointer = internal unnamed_addr global void (i32, i32, i32, i8**)* null, align 8

; Function attributes: "frame-pointer"="none" "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx16,+cx8,+fxsr,+mmx,+popcnt,+sse,+sse2,+sse3,+sse4.1,+sse4.2,+ssse3,+x87" "tune-cpu"="generic" uwtable willreturn writeonly
define void @xamarin_app_init (void (i32, i32, i32, i8**)* %fn) local_unnamed_addr #0
{
	store void (i32, i32, i32, i8**)* %fn, void (i32, i32, i32, i8**)** @get_function_pointer, align 8
	ret void
}

; Names of classes in which marshal methods reside
@mm_class_names = local_unnamed_addr constant [0 x i8*] zeroinitializer, align 8
@__MarshalMethodName_name.0 = internal constant [1 x i8] c"\00", align 1

; mm_method_names
@mm_method_names = local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	; 0
	%struct.MarshalMethodName {
		i64 0, ; id 0x0; name: 
		i8* getelementptr inbounds ([1 x i8], [1 x i8]* @__MarshalMethodName_name.0, i32 0, i32 0); name
	}
], align 16; end of 'mm_method_names' array


attributes #0 = { "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" uwtable willreturn writeonly "frame-pointer"="none" "target-cpu"="x86-64" "target-features"="+cx16,+cx8,+fxsr,+mmx,+popcnt,+sse,+sse2,+sse3,+sse4.1,+sse4.2,+ssse3,+x87" "tune-cpu"="generic" }
attributes #1 = { "min-legal-vector-width"="0" mustprogress "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" uwtable "frame-pointer"="none" "target-cpu"="x86-64" "target-features"="+cx16,+cx8,+fxsr,+mmx,+popcnt,+sse,+sse2,+sse3,+sse4.1,+sse4.2,+ssse3,+x87" "tune-cpu"="generic" }
attributes #2 = { nounwind }

!llvm.module.flags = !{!0, !1}
!llvm.ident = !{!2}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!2 = !{!"Xamarin.Android remotes/origin/d17-5 @ 45b0e144f73b2c8747d8b5ec8cbd3b55beca67f0"}
!llvm.linker.options = !{}
