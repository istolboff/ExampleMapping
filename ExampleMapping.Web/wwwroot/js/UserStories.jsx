var TransientState = {
    Created: {},
    Deleted: {}
};

var constRuleType = "95D959BF-8F42-4F22-BBDA-2CC38BFA9548";
var constExampleType = "A24BC9D5-4DB9-42BF-86DE-E31C0C585044";
var constQuestionType = "AA9B20EA-8C80-493D-AE16-5B77659C9FCD";

class DeletableItemBase extends React.Component {
    constructor(props) {
        super(props);
        this.state = { data: props.data };
        this.deleteSelf = this.deleteSelf.bind(this);
    }

    deleteSelf() {
        this.setState(prevState => {
            if (this.state.data.Id) {
                prevState.data.Id = -prevState.data.Id;
            } else {
                prevState.data.transientState = TransientState.Deleted;
            }
            return prevState;
        });
    }
}

class Example extends DeletableItemBase {
    constructor(props) {
        super(props);
    }

    componentDidMount() {
        ReactDOM.findDOMNode(this.refs.newlyCreatedExample).focus();
    }

    render() {
        return (this.state.data.transientState !== TransientState.Deleted &&
            <div className="exampleElementsGroup">
                <input 
                       type="hidden" 
                       name={"Rules[" + this.props.modelBindingRuleIndex + "].Examples[" + this.props.modelBindingExampleIndex + "].Id"} 
                       defaultValue={this.props.data.Id} />
                <input 
                       className="exampleWording" 
                       type="text" 
                       name={"Rules[" + this.props.modelBindingRuleIndex + "].Examples[" + this.props.modelBindingExampleIndex + "].Name"}
                       defaultValue={this.props.data.Name} 
                       placeholder="Enter example text here" 
                       ref="newlyCreatedExample" />
                <input type="button" className="deleteExample" value="X" onClick={this.deleteSelf}/>
            </div>);
    }
}

function Examples(props) {
    return <div>{
            props.data.map((example, exampleIndex) => 
                <Example key={exampleIndex} data={example} modelBindingRuleIndex={props.modelBindingRuleIndex} modelBindingExampleIndex={exampleIndex }/>)
           }</div>;
}

class Rule extends DeletableItemBase {
    constructor(props) {
        super(props);
    }

    componentDidMount() {
        ReactDOM.findDOMNode(this.refs.newlyCreatedRule).focus();
    }

    render() {
        return ((this.state.data.transientState !== TransientState.Deleted || this.state.data.Id < 0) &&
                <div 
                     className="ruleElementsGroup"
                     onDragOver={
                        dragEvent => {
                             if (dragEvent.dataTransfer.getData("text") === constExampleType) {
                                  dragEvent.preventDefault();
                             }
                        }
                    }

                    onDrop={
                        dropEvent => {
                            if (dropEvent.dataTransfer.getData("text") === constExampleType) {
                                dropEvent.preventDefault();
                                this.setState(prevState => {
                                    prevState.data.Examples.push({ Id: 0, Name: '' });
                                    return prevState;
                                });
                            }
                        }
                    }>
                   <input type="hidden" name={"Rules[" + this.props.modelBindingRuleIndex + "].Id"} defaultValue={this.props.data.Id} />
                   <input className="ruleWording" 
                          type="text" 
                          name={"Rules[" + this.props.modelBindingRuleIndex + "].Name"} 
                          defaultValue={this.props.data.Name} 
                          placeholder="Enter rule text here" ref="newlyCreatedRule" />
                   <input type="button" className="deleteRule" value="X" onClick={this.deleteSelf} />
                   <Examples data={this.props.data.Examples} modelBindingRuleIndex={this.props.modelBindingRuleIndex} />
                </div>);
    }
}

function Rules(props) {
    return <div className="RulesContainer">{ props.data.map((rule, ruleIndex) => <Rule key={ruleIndex} data={rule} modelBindingRuleIndex={ruleIndex} />) }</div>;
}

class Question extends DeletableItemBase {
    constructor(props) {
        super(props);
    }

    componentDidMount() {
        ReactDOM.findDOMNode(this.refs.newlyCreatedQuestion).focus();
    }

    render() {
        return ((this.state.data.transientState !== TransientState.Deleted || this.state.data.Id < 0) && 
            <div className="questionElementsGroup">
                <input type="hidden" name={"Questions[" + this.props.modelBindingQuestionIndex + "].Id"} defaultValue={this.props.data.Id} />
                <input 
                       className="questionWording" 
                       type="text" 
                       name={"Questions[" + this.props.modelBindingQuestionIndex + "].Name"}
                       defaultValue={this.props.data.Name} 
                       placeholder="Enter question text here" 
                       ref="newlyCreatedQuestion" />
                <input type="button" className="deleteQuestion" value="X" onClick={this.deleteSelf} />
            </div>);
    }
}

function Questions(props) {
    return <div className="QuestionsContainer">{ props.data.map((question, questionIndex) => <Question key={questionIndex} data={question} modelBindingQuestionIndex={questionIndex} />) }</div>;
}

function UserStory(props) {
    return <div>
                <div className="UserStoryName"><input type="text" name="Name" defaultValue={props.data.Name} /></div>
                <Rules data={props.data.Rules} />
                <Questions data={props.data.Questions} />
            </div>;
}

function EditingControls(props) {
    return <div className="EditingControlsContainer">
               <a id="AddNewRule" href="#" onClick={props.addNewRule} onDragStart={dragEvent => dragEvent.dataTransfer.setData("text", constRuleType)}>Add Rule</a>
               <a id="AddNewExample" href="#" onDragStart={dragEvent => dragEvent.dataTransfer.setData("text", constExampleType)}>Add Example</a>
               <a id="AddNewQuestion" href="#" onClick={props.addNewQuestion} onDragStart={dragEvent => dragEvent.dataTransfer.setData("text", constQuestionType)}>Add Question</a>
            </div>;
}

class UserStoryEditForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = { data: props.data };
        this.addNewRule = this.addNewRule.bind(this);
        this.addNewQuestion = this.addNewQuestion.bind(this);
    }

    addNewRule() {
        this.setState(prevState => {
            prevState.data.Rules.push({ Id: 0, Name: '', Examples: [] });
            return prevState;
        });
    }

    addNewQuestion() {
        this.setState(prevState => {
            prevState.data.Questions.push({ Id: 0, Name: '' });
            return prevState;
        });
    }

    render() {
        return <div>
                    <UserStory data={this.state.data} />
                    <EditingControls addNewRule={this.addNewRule} addNewQuestion={this.addNewQuestion} />
        </div>;
    }
}