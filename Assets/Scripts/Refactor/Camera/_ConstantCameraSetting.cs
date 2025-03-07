using UnityEngine;

namespace Core.GamePlay{
    public static class _ConstantCameraSetting{
        public static Vector3 _9x21PositionSetting = new Vector3(0, 0.3f, -13);
        public static Vector3 _9x16PositionSetting = new Vector3(0, 0, -10);
    
        private static Vector3 _maxSizeIs5Setting = new Vector3(0, 0, -12);
        private static Vector3 _maxSizeIs10Setting = new Vector3(0, 0, -18);
        private static Vector3 _maxSizeIs15Setting = new Vector3(0, 0, -27);
        private static Vector3 _maxSizeIs20Setting = new Vector3(0, 0, -35);

        private static Vector3 _maxSizeCameraZoomIs5 = new Vector3(0, 0, -7f);
        private static Vector3 _maxSizeCameraZoomIs10 = new Vector3(0, 0, -10f);
        private static Vector3 _maxSizeCameraZoomIs15 = new Vector3(0, 0, -18f);
        private static Vector3 _maxSizeCameraZoomIs20 = new Vector3(0, 0, -26f);

        private static Vector3 _minSizeCameraZoomIs5 = new Vector3(0, 0, -18f);
        private static Vector3 _minSizeCameraZoomIs10 = new Vector3(0, 0, -26f);
        private static Vector3 _minSizeCameraZoomIs15 = new Vector3(0, 0, -36f);
        private static Vector3 _minSizeCameraZoomIs20 = new Vector3(0, 0, -44f);

        public static (Vector3, Vector3, Vector3) GetCameraPositionValue(int size){
            if(size <= 5){
                return (_maxSizeIs5Setting, _maxSizeCameraZoomIs5, _minSizeCameraZoomIs5);
            }else if(size <= 10){
                return (_maxSizeIs10Setting, _maxSizeCameraZoomIs10, _minSizeCameraZoomIs10);
            }else if(size <= 15){
                return (_maxSizeIs15Setting , _maxSizeCameraZoomIs15, _minSizeCameraZoomIs15);
            }else 
                return (_maxSizeIs20Setting, _maxSizeCameraZoomIs20, _minSizeCameraZoomIs20);
        }
    }
}