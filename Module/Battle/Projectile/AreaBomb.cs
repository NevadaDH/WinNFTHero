
using UnityEngine;
namespace Mao 
{
    public class AreaBomb : MonoBehaviour
    {
        public const float g = 9.8f * 2.0F;
        public Vector3 targetPos;  
        public float speed = 10;  
        private float verticalSpeed;  
        private Vector3 moveDirection;  
        
        private float angleSpeed;  
        private float angle;  
        private float time;  

        public delegate void xxFlowHandler(Vector3 pos, bool stun, float afterDurationExplosion);
        bool _stun;
        float _afterDurationExplosion; 
        protected xxFlowHandler PlayEnd;

        bool isStart = false;

        void Start()  
        {}  

        public float excute(xxFlowHandler handler, bool stun, float afterDurationExplosion)
        {
            PlayEnd = handler;
            _stun = stun;
            _afterDurationExplosion = afterDurationExplosion;

            float tmepDistance = Vector3.Distance(transform.position, targetPos);  
            float tempTime = tmepDistance / speed;  
            float riseTime, downTime;  
            riseTime = downTime = tempTime / 2;  
            verticalSpeed = g * riseTime;  
            transform.LookAt(targetPos);  

            float tempTan = verticalSpeed / speed;  
            double hu = Mathf.Atan(tempTan);  
            angle = (float)(180 / Mathf.PI * hu);   //  （将要转换的放在分子上） 或者直接使用 hu * Mathf.Rad2Deg
            transform.eulerAngles = new Vector3(-angle, transform.eulerAngles.y, transform.eulerAngles.z);  
            angleSpeed = angle / riseTime;  
            moveDirection = targetPos - transform.position;  
            isStart = true;
            return tempTime*2;
        }

        void Update()  
        {  
            if(!isStart)
              return;

            if(transform.position.y < targetPos.y)  
            {  
                Vector3 pos = new Vector3(transform.position.x, 0.5F, transform.position.z);
                if(PlayEnd != null)
                   PlayEnd(pos, _stun, _afterDurationExplosion);
                //finish  
                InstanceManager.Instance.DeCreate(transform);
                return;  
            }

            time += Time.deltaTime;  
            float test = verticalSpeed - g * time;  
            transform.Translate(moveDirection.normalized * speed * Time.deltaTime, Space.World);  
            transform.Translate(Vector3.up * test * Time.deltaTime,Space.World);  
            float testAngle = -angle + angleSpeed * time;  
            transform.eulerAngles = new Vector3(testAngle, transform.eulerAngles.y, transform.eulerAngles.z);  
        }
    }
}
