using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using MadoriVR.Scripts.CreateHouseModel.Common;
using VContainer;
using VContainer.Unity;

namespace MadoriVR.Scripts.CreateHouseModel.Core
{
    // TODO: クラス名見直す
    public sealed class SequenceController : IAsyncStartable
    {
        private readonly ImageShower imageShower;
        private readonly LineDrawController lineDrawController;
        
        [Inject]
        public SequenceController(ImageShower imageShower, LineDrawController lineDrawController)
        {
            this.imageShower = imageShower;
            this.lineDrawController = lineDrawController;
        }
        
        public async UniTask StartAsync(CancellationToken cancellation)
        {
            // TODO: 表示方法もこちらから指定するのが自然。UIの非表示もこっちで指示できるし。
            await imageShower.LoadImage();
            
            Log.Info($"Image loaded.");

            var lines = await lineDrawController.DrawLines();
            
            Log.Info($"Lines: {lines.Count()}");

            var generator = new HouseModelGenerator();
            var houseModel = generator.Generate(lines);

            Log.Info($"House model was generated.");
            
            // 次のシーン読みこみ？ + houseModelの引き渡し？
        }
    }
}