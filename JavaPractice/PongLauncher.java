package application;

import javafx.application.Application;
import javafx.stage.Stage;
import javafx.scene.Scene;
import javafx.scene.input.MouseEvent;

public class PongLauncher extends Application{
	 @Override // Override the start method in the Application class
	    public void start(Stage primaryStage) {
	    	Paddle paddle = new Paddle(); //this organization is awful on my part
	    	GameManager manager = new GameManager(paddle);
	        BallPane ballPane = new BallPane(manager); // Create a ball pane

	        manager.setCenter(ballPane); //manager UI controls player camera
	        manager.getChildren().add(paddle);
	        

	        // Create a scene and place it in the stage
	        Scene scene = new Scene(manager, 500, 300);
	        scene.addEventFilter(MouseEvent.MOUSE_MOVED, e -> paddle.followMouse(e));
	        primaryStage.setTitle("Pong"); // Set the stage title
	        primaryStage.setScene(scene); // Place the scene in the stage
	        primaryStage.show(); // Display the stage

	        // Must request focus after the primary stage is displayed
	        ballPane.requestFocus();
	    }

	    /**
	    * The main method is only needed for the IDE with Limited
	    * JavaFX support. Not needed for running from the command line.
	    */
	    public static void main(String[] args) {
	        launch(args);
	    }
}
