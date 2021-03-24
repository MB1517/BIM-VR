import * as THREE from './libs/three/three.module.js';
import { VRButton } from './libs/three/jsm/VRButton.js';
import { XRControllerModelFactory } from './libs/three/jsm/XRControllerModelFactory.js';
import { BoxLineGeometry } from './libs/three/jsm/BoxLineGeometry.js';
import { Stats } from './libs/stats.module.js';
import { OrbitControls } from './libs/three/jsm/OrbitControls.js';
import {GLTFLoader} from 'https://threejsfundamentals.org/threejs/resources/threejs/r125/examples/jsm/loaders/GLTFLoader.js';
import { ColladaLoader } from './libs/three/loaders/ColladaLoader.js';

class App{
    
	constructor(){
        let elf;
        var mesh, curMesh;
        var imgNum = 0;
        var maxImgNum = 21;       

        var posX = 30;
        var posY = 1.6;
        var posZ = 70;
        this.walkspeed = 5;

		const container = document.createElement( 'div' );
		document.body.appendChild( container );
        
        this.clock = new THREE.Clock();
        //camera
		this.camera = new THREE.PerspectiveCamera( 45, window.innerWidth / window.innerHeight, 0.1, 20000 );		
        this.camera.position.set( posX, posY, posZ );
		this.scene = new THREE.Scene();        
        console.log({element: this.scene.background})
        // const light0 = new THREE.HemisphereLight( 0xffffff, 0x404040 );
        // light0.intensity = 0.5;
		// this.scene.add(light0);
        // const light = new THREE.DirectionalLight( 0xffffff );
        // light.position.set( 1, 1, 1 ).normalize();
        // light.intensity = 0.5;
		// this.scene.add( light );
        
		this.renderer = new THREE.WebGLRenderer({ antialias: true } );
		this.renderer.setPixelRatio( window.devicePixelRatio );
		this.renderer.setSize( window.innerWidth, window.innerHeight );
        this.renderer.outputEncoding = THREE.sRGBEncoding;
        
		container.appendChild( this.renderer.domElement );
        
        this.controls = new OrbitControls( this.camera, this.renderer.domElement );
        this.controls.target.set(0, 4, 0);
        this.controls.update();
        
        this.stats = new Stats();
        
        this.raycaster = new THREE.Raycaster();
        this.workingMatrix = new THREE.Matrix4();
        this.workingVector = new THREE.Vector3();
        this.origin = new THREE.Vector3();
        
        // this.initScene(imgNum,mesh);
        
        var path = "./pano/"
        var name = path+"CS1_ ("+imgNum+").jpg";
        // var imgName = document.getElementById('imgName');
        // imgName.textContent = name;

        var geometry = new THREE.SphereBufferGeometry( 500, 60, 40 );
        geometry.scale( - 1, 1, 1 );
        var texture = new THREE.TextureLoader().load(name);
        var material = new THREE.MeshBasicMaterial( { map: texture } );
        mesh = new THREE.Mesh( geometry, material );
        curMesh = mesh;
        this.scene.add(mesh);

        // var bNext = document.getElementById('bnxt')
        // var bPrev = document.getElementById('bprv')
        // bNext.addEventListener('mousedown', nextImage, false);
        // bPrev.addEventListener('mousedown', prevImage, false);

        this.setupVR();
        
        window.addEventListener('resize', this.resize.bind(this) );
        
        this.renderer.setAnimationLoop( this.render.bind(this));
        // animate();
        function nextImage()
        {
            if (imgNum <= maxImgNum){
                imgNum+= 1;
            }
            if(imgNum > maxImgNum){
                imgNum = 0;					
            }
            name  = "./pano/CS1_ ("+imgNum+").jpg";
            imgName.textContent = name;

            var newtexture = new THREE.TextureLoader().load(name);
            var newmaterial = new THREE.MeshBasicMaterial( { map: newtexture } );
            var newmesh = new THREE.Mesh( geometry, newmaterial );
            
            this.scene.remove(curMesh);
            this.scene.add(newmesh);
            curMesh = newmesh;

        }
        function prevImage(){				
            if(imgNum == 0){
                imgNum = maxImgNum;
            }
            else{imgNum -= 1;}
            name  = "./pano/CS1_ ("+imgNum+").jpg";
            imgName.textContent = name;
            var newtexture = new THREE.TextureLoader().load(name);
            var newmaterial = new THREE.MeshBasicMaterial( { map: newtexture } );
            var newmesh = new THREE.Mesh( geometry, newmaterial );

            this.scene.remove(curMesh);
            this.scene.add(newmesh);
            curMesh = newmesh;
        }
	}
    	
    
    
    random( min, max ){
        return Math.random() * (max-min) + min;
    }

    initScene(imgNum,mesh,curMesh){

        var maxImgNum = 21;
        var path = "./pano/"
        var name = path+"CS1_ ("+imgNum+").jpg";

        var geometry = new THREE.SphereBufferGeometry( 500, 60, 40 );
        geometry.scale( - 1, 1, 1 );
        var texture = new THREE.TextureLoader().load(name);
        var material = new THREE.MeshBasicMaterial( { map: texture } );
        mesh = new THREE.Mesh( geometry, material );
        curMesh = mesh;
        this.scene.add(mesh);
    } 

    
    setupVR(){

        this.renderer.xr.enabled = true;        
        const button = new VRButton( this.renderer )

        var vrbutton = VRButton.createButton(this.renderer)
        vrbutton.addEventListener( 'selectstart', onSelectStart );
        
        
        document.body.appendChild(vrbutton);
        
        var vrButton = document.getElementById('VRButton')    ;

        const self = this;        

        function onSelectStart() {            
            this.userData.selectPressed = true;
        }
        function onSelectEnd() {
            this.userData.selectPressed = false;         
        }
        
        this.controller = this.renderer.xr.getController( 0 );
        // this.controller.addEventListener( 'select', onSelectStart );
        // this.controller.addEventListener( 'selectend', onSelectEnd );
        // this.controller.addEventListener( 'touchdown', onSelectStart );
        // this.controller.addEventListener( 'touchup', onSelectEnd );
        
        this.scene.add( this.controller );
        const controllerModelFactory = new XRControllerModelFactory();
        this.controllerGrip = this.renderer.xr.getControllerGrip( 0 );
        this.controllerGrip.add( controllerModelFactory.createControllerModel( this.controllerGrip ) );
        this.scene.add( this.controllerGrip );
        
    }
    handleController( controller, dt ){
        if (controller.userData.selectPressed ){ 
            
        }
    }    
    resize(){
        this.camera.aspect = window.innerWidth / window.innerHeight;
        this.camera.updateProjectionMatrix();
        this.renderer.setSize( window.innerWidth, window.innerHeight );  
    }
    
	render() { 
        // const dt = this.clock.getDelta();
        // this.stats.update();
        // // this.dolly.position.set( 500, 1.6, 500 );
        // if (this.controller ) this.handleController( this.controller, dt );
        this.renderer.render( this.scene, this.camera );
    }
}

export { App };