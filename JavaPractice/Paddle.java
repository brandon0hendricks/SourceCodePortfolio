package application;

import javafx.scene.input.MouseEvent;
import javafx.scene.paint.Color;
import javafx.scene.shape.Rectangle;

public class Paddle extends Rectangle {

	public Paddle() {
		super(0,25, 10, 75); //this calls the constructor for rectangle
		setX(0); //Restrict the rectangle to the left side
		setFill(Color.GREEN);
	}
 
	void followMouse(MouseEvent mouseInput) {
		 double mouseYPos = mouseInput.getY() - getHeight() / 2; //centers paddle
		
		setY(mouseYPos); //sets rectangle
	}
	        
}
