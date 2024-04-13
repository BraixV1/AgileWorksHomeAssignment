import React from "react";


const LoadingScreen: React.FC = () => {
    return (
        <div className="text-center">
            <div className="mx-auto spinner-border" role="status">
                <span className="sr-only"></span>
            </div>
        </div>
    );
};

export default LoadingScreen;