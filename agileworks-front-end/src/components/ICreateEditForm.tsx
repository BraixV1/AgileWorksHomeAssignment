'use client'
import React from 'react';
import { useForm } from 'react-hook-form';
import { useRouter } from 'next/navigation';
import { IFormInterface } from '@/components/IFormInterface';
import Link from 'next/link';


interface TaskFormProps {
    defaultValues: IFormInterface;
    onSubmit: (data: IFormInterface) => Promise<void>;
}

const ICreateEditForm: React.FC<TaskFormProps> = ({ defaultValues, onSubmit }) => {
    const { register, handleSubmit } = useForm<IFormInterface>();
    const router = useRouter();
  
    const submitHandler = async (data: IFormInterface) => {
      try {
        await onSubmit(data);
        router.push('/');
      } catch (error) {
        console.error('Error:', error);
      }
    };
  
    return (
      <form onSubmit={handleSubmit(submitHandler)}>
        <div className="form-group">
          <label htmlFor="description">Description</label>
          <input {...register('description')} defaultValue={defaultValues.description} className="form-control" type="text" id="description" />
          <span className="text-danger"></span>
        </div>
  
        <div className="form-group">
          <label htmlFor="hasToBeDoneAtDt">HasToBeDoneAtDt</label>
          <input {...register('hasToBeDoneAtDt')} defaultValue={defaultValues.hasToBeDoneAtDt instanceof Date ? defaultValues.hasToBeDoneAtDt.toISOString() : defaultValues.hasToBeDoneAtDt} className="form-control" type="datetime-local" id="hasToBeDoneAtDt" />
          <span className="text-danger"></span>
        </div>
  
        <div className="form-group">
          <input type="submit" value="Submit" className="btn btn-primary" />
        </div>
        <input {...register('id')} type="hidden" value={defaultValues.id} />
        <Link href="../">Back to list</Link>
      </form>
    );
};
export default ICreateEditForm;